using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace DlaGrzesia.Objects.UI
{
    public class AvatarDisplay : IObject
    {
        private readonly Tileset tileset;
        private readonly Tileset dogTileset;
        private readonly SpriteFont font;
        private readonly Point location;
        private readonly KonamiCodeSequence konamiCode = new KonamiCodeSequence();
        private Counter dogDuration = Counter.NewElapsed();
        private Counter dogFrameDuration = Counter.NewStarted(3).ToCyclic();
        private Counter safeShutdownDuration = Counter.NewElapsed(150);

        public AvatarDisplay(Tileset tileset, Tileset dogTileset, SpriteFont font, Point location)
        {
            this.tileset = tileset;
            this.dogTileset = dogTileset;
            this.font = font;
            this.location = location;
        }

        public bool Expired => false;

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
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

            if (modifiers.IncludeDebugData)
                spriteBatch.DrawString(
                    font, 
                    konamiCode.CurrentKeyIndex.ToString(), 
                    location.ToVector2(), 
                    Color.OrangeRed);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            if (!dogDuration.Elapsed)
            {
                dogFrameDuration.Tick();
                if (dogFrameDuration.Elapsed)
                    dogDuration.Tick();
            }

            safeShutdownDuration = safeShutdownDuration.Tick();

            if (environmentState.Input.JustPressedKeys.Count == 1)
            {
                var pressedKey = environmentState.Input.JustPressedKeys.SingleOrDefault();
                if (pressedKey != Keys.None && konamiCode.Update(pressedKey))
                {
                    dogDuration = Counter.NewStarted(dogTileset.TilesCount);
                }
            }

            if (environmentState.Events.GameSaved)
            {
                safeShutdownDuration = safeShutdownDuration.Reset();
            }
        }
    }
}
