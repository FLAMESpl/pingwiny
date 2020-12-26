using DlaGrzesia.Objects.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DlaGrzesia.Objects.Particles
{
    public class ParticleGenerator : IObject, IGenerator
    {
        private readonly ParticlePrototype particlePrototype;

        public ParticleGenerator(ParticlePrototype particlePrototype)
        {
            this.particlePrototype = particlePrototype;
        }

        public bool Expired { get; set; }

        public Queue<IObject> SpawnedObjects { get; } = new Queue<IObject>();

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
        }

        public void Spawn(Point position)
        {
            var particle = new Particle(particlePrototype, position);
            SpawnedObjects.Enqueue(particle);
        }
    }
}
