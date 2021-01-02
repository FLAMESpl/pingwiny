using DlaGrzesia.Assets;
using DlaGrzesia.Environment;
using DlaGrzesia.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace DlaGrzesia.Objects.UI
{
    public class AvatarDisplay : GameObject
    {
        private Tileset tileset;
        private Tileset dogTileset;
        private SpriteFont font;
        private readonly Point location;
        private readonly KonamiCodeSequence konamiCode = new KonamiCodeSequence();
        private Counter dogDuration = Counter.NewElapsed();
        private Counter dogFrameDuration = Counter.NewStarted(3).ToCyclic();
        private Counter safeShutdownDuration = Counter.NewElapsed(150);

        public AvatarDisplay(Point location)
        {
            this.location = location;
        }

        protected override void OnInitialized()
        {
            var textures = Environment.Resources.Textures;
            tileset = textures.Grzesiek;
            dogTileset = textures.DOG;
            font = Environment.Resources.Fonts.Standard;
        }

        public override void Draw(GameTime elapsed, SpriteBatch spriteBatch)
        {
            if (!safeShutdownDuration.Elapsed)
            {
                spriteBatch.Draw(tileset, location, 1, LayerDepths.UI);
            }
            else if (!dogDuration.Elapsed)
            {
                spriteBatch.Draw(dogTileset, location, dogDuration.CurrentValueReversed, LayerDepths.UI);
            }
            else
            {
                spriteBatch.Draw(tileset, location, 0, LayerDepths.UI);
            }

            if (Environment.IsDebugDataOn)
                spriteBatch.DrawString(
                    font, 
                    konamiCode.CurrentKeyIndex.ToString(), 
                    location.ToVector2(), 
                    Color.OrangeRed);
        }

        public override void Update(GameTime elapsed)
        {
            if (!dogDuration.Elapsed)
            {
                dogFrameDuration.Tick();
                if (dogFrameDuration.Elapsed)
                    dogDuration.Tick();
            }

            safeShutdownDuration = safeShutdownDuration.Tick();

            if (Environment.Input.CanHandleKeyboardKeys)
            {
                var pressedKey = Environment.Input.JustPressedKeys.FirstOrDefault();
                if (pressedKey != Keys.None)
                {
                    Environment.Input.HandleKeyboardKeys();
                    if (konamiCode.Update(pressedKey))
                        dogDuration = Counter.NewStarted(dogTileset.TilesCount);
                }
            }

            if (GameState.Events.TryGetFirst<GameSaved>(out _))
            {
                safeShutdownDuration = safeShutdownDuration.Reset();
            }
        }
    }
}
