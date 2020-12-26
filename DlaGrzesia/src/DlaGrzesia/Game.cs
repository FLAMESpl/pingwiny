using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects;
using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Objects.UI;
using DlaGrzesia.Scoring;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Fonts fonts;
        private Textures textures;
        private Point stageSize = new Point(1000, 700);
        private List<IObject> objects = new List<IObject>();
        private readonly List<IObject> uiElements = new List<IObject>();
        private readonly InputManager inputManager = new InputManager();
        private bool includeDebugData = false;
        private Point stageLocation = new Point(15, 15);
        private bool paused = false;
        private ParticleGenerator heartsGenerator;
        private Score score = new Score(0);
        private readonly CyclicList<IEnumerable<Keys>> pressedKeys = new CyclicList<IEnumerable<Keys>>(12, Array.Empty<Keys>());
        private IReadOnlyList<UpgradeState> upgrades;
        private MoneyDebugInput moneyDebugInput = new MoneyDebugInput();

        private Rectangle StageBounds => new Rectangle(stageLocation, stageSize);

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            upgrades = new List<UpgradeState>
            {
                new UpgradeState(1),
                new UpgradeState(10),
                new UpgradeState(100),
                new UpgradeState(500),
                new UpgradeState(1000),
                new UpgradeState(4000),
                new UpgradeState(10000),
                new UpgradeState(100000)
            };
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var resourcesLoader = new ResourcesLoader(Content);
            textures = resourcesLoader.Load<Textures>();
            fonts = resourcesLoader.Load<Fonts>();

            _graphics.PreferredBackBufferWidth = textures.UIBackground.Width;
            _graphics.PreferredBackBufferHeight = textures.UIBackground.Height;
            _graphics.ApplyChanges();

            heartsGenerator = new ParticleGenerator(textures.Heart);
            objects.Add(heartsGenerator);
            objects.Add(new PenguinGenerator(textures, fonts, heartsGenerator));

            InitializeUI();
        }

        protected override void UnloadContent()
        {
            textures?.Dispose();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var inputInfo = inputManager.Update(Window);

            if (inputInfo.JustPressedKeys.Any())
                pressedKeys.Write(inputInfo.JustPressedKeys);

            var gameSaved = false;

            if (inputInfo.Keyboard.IsKeyDown(Keys.LeftControl))
            {
                if (inputInfo.IsKeyJustPressed(Keys.D))
                {
                    includeDebugData = !includeDebugData;
                }
                else if (inputInfo.IsKeyJustPressed(Keys.S))
                {
                    var repository = new GameStateRepository();
                    var state = new GameState(objects.OfType<ISerializable>().ToList(), score.Total);
                    repository.Save(state);
                    gameSaved = true;
                }
                else if (inputInfo.IsKeyJustPressed(Keys.P))
                {
                    paused = !paused;
                }
                else if (inputInfo.IsKeyJustPressed(Keys.L))
                {
                    var repository = new GameStateRepository();
                    TryLoadGame();
                }
                else if (inputInfo.IsKeyJustPressed(Keys.M))
                {
                    moneyDebugInput.Activate(score);
                }    
            }

            var events = new Events(gameSaved);
            var upgradeStates = upgrades.Select(x => x.ToFrameState()).ToList();

            var environmentState = new EnvironmentState(
                StageBounds, 
                inputInfo, 
                score, 
                events, 
                new Upgrades(upgradeStates), 
                moneyDebugInput,
                heartsGenerator);

            var nextObjects = new List<IObject>(objects.Count);

            if (!paused)
            {
                foreach (var @object in objects.Where(static x => !x.Expired))
                {
                    @object.Update(gameTime, environmentState);

                    if (@object is IGenerator generator)
                    {
                        while (generator.SpawnedObjects.TryDequeue(out var spawned))
                            nextObjects.Add(spawned);
                    }

                    nextObjects.Add(@object);
                }

                objects.Clear();
                objects = nextObjects;
            }

            foreach (var element in uiElements)
                element.Update(gameTime, environmentState);

            score.Update();

            foreach (var boughtUpgrade in environmentState.Upgrades.BoughtUpgrades)
                upgrades[boughtUpgrade].LevelUp();

            moneyDebugInput.Update(inputInfo);
            if (moneyDebugInput.NewScore != null)
                score = moneyDebugInput.NewScore;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var modifiers = new DrawingModifiers(includeDebugData);

            _spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);
            _spriteBatch.Draw(textures.Stage, stageLocation.ToVector2(), null, Color.White, 0, default, 1f, SpriteEffects.None, LayerDepths.StageBackground);

            foreach (var @object in objects)
                @object.Draw(gameTime, _spriteBatch, modifiers);

            if (includeDebugData)
            {
                _spriteBatch.DrawStringLines(fonts.Font, stageLocation,
                    (objects.Count.ToString(), Color.Red),
                    (uiElements.Count.ToString(), Color.Black),
                    (gameTime.ElapsedGameTime.TotalMilliseconds.ToString(), Color.DarkCyan));

                var scale = 0.5f;
                var glyphs = fonts.Font.GetGlyphs();
                float keyY = StageBounds.Top + fonts.Font.LineSpacing * 3;
                var color = Color.DarkRed;

                foreach (var keyFrame in pressedKeys.ReadAll().Where(static k => k.Any()))
                {
                    float previousKeyX = StageBounds.Left + 5;
                    keyY += fonts.Font.LineSpacing * scale;

                    foreach (var key in keyFrame)
                    {
                        var text = key.ToString();
                        _spriteBatch.DrawString(fonts.Font, text, new Vector2(previousKeyX, keyY), color, 0, default, scale, default, default);
                        color = color == Color.DarkRed ? Color.Black : Color.DarkRed;
                        previousKeyX += text.Sum(ch => glyphs[ch].WidthIncludingBearings * scale) + 10;
                    }
                }
            }

            _spriteBatch.Draw(textures.UIBackground, Vector2.Zero, null, Color.White, 0, default, 1f, SpriteEffects.None, LayerDepths.UIBackground);

            foreach (var element in uiElements)
                element.Draw(gameTime, _spriteBatch, modifiers);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool TryLoadGame()
        {
            var repository = new GameStateRepository();
            if (repository.FileExists)
            {
                var state = repository.Load(GetDeserializerFactories(heartsGenerator));
                score = new Score(state.TotalScore);
                objects = state.Objects.OfType<IObject>().ToList();
                objects.Add(heartsGenerator);
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerable<IDeserializerFactory> GetDeserializerFactories(ParticleGenerator heartsGenerator)
        {
            yield return new PenguinGeneratorDeserializerFactory(textures, fonts, heartsGenerator);
            yield return new SlidingPenguinDeserializationFactory(textures, fonts, heartsGenerator);
            yield return new SurfingPenguinDeserializationFactory(textures, fonts, heartsGenerator);
            yield return new WalkingPenguinDeserializationFactory(textures, fonts, heartsGenerator);
        }

        private void InitializeUI()
        {
            var scoreLocation = new Point(StageBounds.Right + 30, 30);
            var scoreDisplay = new ScoreDisplay(textures.Hearts, fonts.Font, scoreLocation);
            uiElements.Add(scoreDisplay);

            var avatarLocation = new Point(StageBounds.Right - textures.Grzesiek.TileSize.X, StageBounds.Bottom + 15);
            var avatarDisplay = new AvatarDisplay(textures.Grzesiek, textures.DOG, fonts.Font, avatarLocation);
            uiElements.Add(avatarDisplay);

            var upgradesGrid = new UpgradesGrid(new Point(1045, 150), new Point(270, 150), 2, new Point(30, 30));
            uiElements.Add(new UpgradeDisplay(textures.Alex, fonts.Font, upgradesGrid.GetIndexBounds(0), 0));
            uiElements.Add(new UpgradeDisplay(textures.Kamil, fonts.Font, upgradesGrid.GetIndexBounds(1), 1));
        }
    }
}
