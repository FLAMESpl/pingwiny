using Microsoft.Xna.Framework;

namespace DlaGrzesia.Objects.Particles
{
    public class HeartsParticle : ParticlePrototype
    {
        public HeartsParticle(bool yellow, Point location) : base(
            yellow ? 1 : 0,
            location,
            new Point(0, yellow ? -1 : -3),
            yellow ? 48 : 24)
        {
        }
    }
}
