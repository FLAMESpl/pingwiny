using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.Particles
{
    public class ParticlePrototype
    {
        public ParticlePrototype(
            Texture2D texture,
            Point speed,
            int lifetime)
        {
            Texture = texture;
            Speed = speed;
            Lifetime = lifetime;
        }

        public Texture2D Texture { get; }
        public Point Speed { get; }
        public int Lifetime { get; }
    }
}
