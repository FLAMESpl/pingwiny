using DlaGrzesia.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DlaGrzesia.Objects.Particles
{
    public class ParticleGenerator : IObject, IGenerator
    {
        private readonly Tileset tileset;

        public ParticleGenerator(Tileset tileset)
        {
            this.tileset = tileset;
        }

        public bool Expired { get; set; }

        public Queue<IObject> SpawnedObjects { get; } = new Queue<IObject>();

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
        }

        public void Spawn(ParticlePrototype prototype)
        {
            var particle = new Particle(tileset, prototype);
            SpawnedObjects.Enqueue(particle);
        }
    }
}
