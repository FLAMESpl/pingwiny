using DlaGrzesia.Assets;
using DlaGrzesia.Environment;
using DlaGrzesia.Objects;
using DlaGrzesia.Objects.UI;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DlaGrzesia
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameEnvironment environment;
        private GameState gameState;
        private readonly ObjectsCollection mechanicsElements = new ObjectsCollection();
        private readonly ObjectsCollection uiElements = new ObjectsCollection();

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var resourcesLoader = new ResourcesLoader(Content);
            var textures = resourcesLoader.Load<Textures>();
            var fonts = resourcesLoader.Load<Fonts>();

            _graphics.PreferredBackBufferWidth = textures.UIBackground.Width;
            _graphics.PreferredBackBufferHeight = textures.UIBackground.Height;
            _graphics.ApplyChanges();

            gameState = new GameState();
            environment = new GameEnvironment(
                new InputInfo(Keyboard.GetState(), Mouse.GetState(Window)),
                new GameResources(textures, fonts),
                false,
                false);

            InitializeMechanics();
            InitializeUI();
            InitializeStage();
        }

        protected override void UnloadContent()
        {
            environment?.Resources?.Dispose();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            environment.Input.Update(Keyboard.GetState(), Mouse.GetState(Window));
            mechanicsElements.Update(gameTime);
            gameState.Stage.Objects.Update(gameTime);
            uiElements.Update(gameTime);
            gameState.Score.Update();
            environment.ExecuteAllCommands(gameState);
            gameState.Events.Progress();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var textures = environment.Resources.Textures;
            var stageColor = environment.IsPaused ? Color.DarkSlateGray : Color.White;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(sortMode: SpriteSortMode.BackToFront);

            _spriteBatch.Draw(textures.Stage, gameState.Stage.Bounds.Location.ToVector2(), null, stageColor, 0, default, 1f, SpriteEffects.None, LayerDepths.StageBackground);
            gameState.Stage.Objects.Draw(gameTime, _spriteBatch);
            _spriteBatch.Draw(textures.UIBackground, Vector2.Zero, null, Color.White, 0, default, 1f, SpriteEffects.None, LayerDepths.UIBackground);
            uiElements.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitializeMechanics()
        {
            mechanicsElements.Initialize(environment, gameState);
            mechanicsElements.Add(new InputManager());
            mechanicsElements.Add(new MoneyDebugInput());
        }

        private void InitializeUI()
        {
            uiElements.Initialize(environment, gameState);

            var textures = environment.Resources.Textures;
            var scoreLocation = new Point(gameState.Stage.Bounds.Right + 30, 30);
            var scoreDisplay = new ScoreDisplay(scoreLocation);

            uiElements.Add(scoreDisplay);

            var avatarDisplay = new AvatarDisplay(new Point(
                gameState.Stage.Bounds.Right - textures.Grzesiek.TileSize.X,
                gameState.Stage.Bounds.Bottom + 15));

            uiElements.Add(avatarDisplay);

            var upgradesGrid = new UpgradesGrid(new Point(1045, 150), new Point(270, 150), 2, new Point(30, 30));

            uiElements.Add(new UpgradeDisplay(textures.Alex, upgradesGrid.GetIndexBounds(0), 0));
            uiElements.Add(new UpgradeDisplay(textures.Kamil, upgradesGrid.GetIndexBounds(1), 1));
            uiElements.Add(new UpgradeDisplay(textures.Marcin, upgradesGrid.GetIndexBounds(2), 2));
            uiElements.Add(new UpgradeDisplay(textures.Marek, upgradesGrid.GetIndexBounds(3), 3));
            uiElements.Add(new UpgradeDisplay(textures.Tymon, upgradesGrid.GetIndexBounds(4), 4));
            uiElements.Add(new DebugOverlay());
        }

        private void InitializeStage()
        {
            gameState.Stage.Reset(environment, gameState);
        }
    }
}
