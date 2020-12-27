using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.Particles
{
    public class Particle : IObject
    {
        private readonly Tileset tileset;
        private readonly ParticlePrototype prototype;
        private Point location;
        private Counter age;

        public Particle(Tileset tileset, ParticlePrototype prototype)
        {
            this.tileset = tileset;
            this.prototype = prototype;
            location = prototype.Position;
            age = Counter.NewStarted(prototype.Lifetime);
        }

        public bool Expired => age.Elapsed;

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            var color = modifiers.IsGamePaused ? Color.DarkSlateGray : Color.White;
            spriteBatch.Draw(tileset, location, prototype.TileIndex, color: color);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            age.Tick();
            location += prototype.Speed;
        }
    }
}
