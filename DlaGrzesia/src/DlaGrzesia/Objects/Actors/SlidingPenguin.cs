using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class SlidingPenguin : IObject, ISerializable
    {
        private readonly int speed = 5;

        public SlidingPenguin(PenguinBase @base)
        {
            Base = @base;
        }

        public bool Expired => Base.Expired;
        private PenguinBase Base { get; }

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            Base.Draw(spriteBatch, modifiers, (int)Base.Orientation.Name);
        }

        public void Deserialize(Stream stream)
        {
        }

        public void Serialize(Stream stream)
        {
            Base.Serialize(stream);
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            Base.Update(environmentState);

            if (!Expired)
            {
                Base.Location += new Point(0, speed);
            }
        }
    }
}
