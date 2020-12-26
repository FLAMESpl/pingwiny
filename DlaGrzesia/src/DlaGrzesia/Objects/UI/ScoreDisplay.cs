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

        public ScoreDisplay(Texture2D texture, SpriteFont font, Point location)
        {
            this.texture = texture;
            this.font = font;
            this.location = location;
        }

        public bool Expired => false;

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            spriteBatch.Draw(texture, location.ToVector2(), Color.White);
            spriteBatch.DrawString(font, points.ToString(), GetTextLocation(), Color.Black);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            points = environmentState.Score.Total;
        }

        private Vector2 GetTextLocation() => new Vector2(
            location.X + texture.Width + distanceBetweenTextAndImage, 
            location.Y);
    }
}
