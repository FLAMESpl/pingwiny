using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.Particles
{
    public class HeartParticle : GameObject
    {
        private readonly int tileIndex;
        private readonly Point speed;
        private Tileset tileset;
        private Point location;
        private Counter remainingLife;

        public HeartParticle(bool yellow, Point location)
        {
            tileIndex = yellow ? 1 : 0;
            speed = new Point(0, yellow ? -1 : -3);
            remainingLife = Counter.NewStarted(yellow ? 48 : 24);
            this.location = location;
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            var color = Environment.IsPaused ? Color.DarkSlateGray : Color.White;
            batch.Draw(tileset, location, tileIndex, color: color);
        }

        public override void Update(GameTime gameTime)
        {
            location += speed;
            remainingLife.Tick();
            if (remainingLife.Elapsed)
                Destroy();
        }

        protected override void OnInitialized()
        {
            tileset = Environment.Resources.Textures.Heart;
        }
    }
}
