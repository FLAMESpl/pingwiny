using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.UI
{
    public class ScoreDisplay : GameObject
    {
        private Texture2D texture;
        private SpriteFont font;
        private readonly Point location;
        private readonly int distanceBetweenTextAndImage = 20;
        private string debugPoints;
        private bool showingDebugInput;

        public ScoreDisplay(Point location)
        {
            this.location = location;
        }

        public override void Draw(GameTime elapsed, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, location.ToVector2(), null, Color.White, 0, default, 1f, SpriteEffects.None, LayerDepths.UI);

            if (showingDebugInput)
                spriteBatch.DrawString(font, debugPoints, GetTextLocation(), Color.DarkMagenta);
            else
                spriteBatch.DrawString(font, GameState.Score.Total.ToString(), GetTextLocation(), Color.Black);
        }

        public override void Update(GameTime elapsed)
        {
            if (showingDebugInput)
            {
                if (GameState.Events.TryGetFirst<ScoreDebugInputDeactivated>(out _))
                    showingDebugInput = false;
                else if (GameState.Events.TryGetFirst<ScoreDebugInputAmountChanged>(out var @event))
                    debugPoints = @event.Amount;

            }
            else if (GameState.Events.TryGetFirst<ScoreDebugInputActivated>(out var @event))
            {
                showingDebugInput = true;
                debugPoints = @event.Amount;
            }
        }

        protected override void OnInitialized()
        {
            var resources = Environment.Resources;
            texture = resources.Textures.Hearts;
            font = resources.Fonts.Standard;
        }

        private Vector2 GetTextLocation() => new Vector2(
            location.X + texture.Width + distanceBetweenTextAndImage, 
            location.Y);
    }
}
