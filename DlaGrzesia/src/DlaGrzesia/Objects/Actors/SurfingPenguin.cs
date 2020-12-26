using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class SurfingPenguin : IObject, ISerializable
    {
        private Counter ascending = Counter.NewStarted(24);
        private readonly int horizontalSpeed = 4;
        private readonly int verticalSpeed = 3;

        public SurfingPenguin(PenguinBase @base)
        {
            Base = @base;
        }

        public bool Expired => Base.Expired;
        private PenguinBase Base { get; }

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            Base.Draw(spriteBatch, modifiers, 0);
        }

        public void Deserialize(Stream stream)
        {
            ascending = stream.ReadStruct<Counter>();
        }

        public void Serialize(Stream stream)
        {
            Base.Serialize(stream);
            stream.WriteStruct(ascending);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            Base.Update(environmentState);

            if (!Expired)
            {
                Base.Location += GetMovement();
                ascending = ascending.Tick();
            }
        }

        private Point GetMovement()
        {
            var direction = ascending.Elapsed ? 1 : -1;
            return new Point(-horizontalSpeed, verticalSpeed * direction);
        }
    }
}
