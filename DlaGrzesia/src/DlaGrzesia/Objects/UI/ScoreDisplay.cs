using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.UI
{
    public class ScoreDisplay : IObject
    {
        private readonly Texture2D texture;
        private readonly SpriteFont font;
        private readonly Point location;
        private readonly int distanceBetweenTextAndImage = 20;
        private int points;
        private string debugPoints;

        public ScoreDisplay(Texture2D texture, SpriteFont font, Point location)
        {
            this.texture = texture;
            this.font = font;
            this.location = location;
        }

        public bool Expired => false;

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            spriteBatch.Draw(texture, location.ToVector2(), null, Color.White, 0, default, 1f, SpriteEffects.None, LayerDepths.UI);

            if (string.IsNullOrWhiteSpace(debugPoints))
                spriteBatch.DrawString(font, points.ToString(), GetTextLocation(), Color.Black);
            else
                spriteBatch.DrawString(font, debugPoints, GetTextLocation(), Color.DarkMagenta);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            points = environmentState.Score.Total;
            debugPoints = environmentState.MoneyDebugInput.CurrentMoneyString;
        }

        private Vector2 GetTextLocation() => new Vector2(
            location.X + texture.Width + distanceBetweenTextAndImage, 
            location.Y);
    }
}
