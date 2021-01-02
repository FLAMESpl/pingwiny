using DlaGrzesia.Assets;
using Microsoft.Xna.Framework;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class SlidingPenguin : PenguinBase
    {
        private readonly int speed = 5;
        private Tileset tileset;

        protected SlidingPenguin() { }

        public SlidingPenguin(
            Point location,
            int duration,
            int scorePerClick,
            int scorePerDestroy) : base(
                new ObjectOrientation(ObjectOrientationName.Down), 
                location, 
                duration,
                scorePerClick,
                scorePerDestroy)
        {
        }

        protected override Tileset Tileset => tileset;

        public override void Update(GameTime elapsed)
        {
            base.Update(elapsed);

            if (IsAlive)
            {
                Location += new Point(0, speed);
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            tileset = Environment.Resources.Textures.PenguinSliding;
        }
    }
}
