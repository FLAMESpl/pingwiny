using DlaGrzesia.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.Particles
{
    public class Particle : IObject
    {
        private readonly ParticlePrototype prototype;
        private Point location;
        private Counter age;

        public Particle(ParticlePrototype prototype, Point location)
        {
            this.prototype = prototype;
            this.location = location;
            age = Counter.NewStarted(prototype.Lifetime);
        }

        public bool Expired => age.Elapsed;

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            spriteBatch.Draw(prototype.Texture, location.ToVector2(), Color.White);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            age.Tick();
            location += prototype.Speed;
        }
    }
}
