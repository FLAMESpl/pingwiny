using DlaGrzesia.Assets;
using Microsoft.Xna.Framework;

namespace DlaGrzesia.Objects.Particles
{
    public class ParticlePrototype
    {
        public ParticlePrototype(
            int tileIndex,
            Point position,
            Point speed,
            int lifetime)
        {
            TileIndex = tileIndex;
            Position = position;
            Speed = speed;
            Lifetime = lifetime;
        }

        public int TileIndex { get; }
        public Point Position { get; }
        public Point Speed { get; }
        public int Lifetime { get; }
    }
}
