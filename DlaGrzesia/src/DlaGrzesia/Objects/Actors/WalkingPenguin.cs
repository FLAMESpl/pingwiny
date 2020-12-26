using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class WalkingPenguin : IObject, ISerializable
    {
        private readonly int speed = 2;
        private Counter animationCountdown = Counter.NewStarted(15).ToCyclic();
        private Counter animationIndex = Counter.NewStarted(3).ToCyclic();
        private readonly Random random = new Random();
        private Counter directionChangeCountdown = Counter.NewStarted(100);

        private Point SpeedVector => new Point(speed);

        public WalkingPenguin(PenguinBase @base)
        {
            Base = @base;
        }

        public bool Expired => Base.Expired;
        private PenguinBase Base { get; }

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            Base.Draw(spriteBatch, modifiers, GetTileIndex());
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            Base.Update(environmentState);

            if (!Expired)
            {
                if (directionChangeCountdown.Elapsed)
                    ChangeDirection();
                else
                    directionChangeCountdown = directionChangeCountdown.Tick();

                animationCountdown = animationCountdown.Tick();
                if (animationCountdown.Elapsed)
                    animationIndex = animationIndex.Tick();

                var movement = Base.Orientation.PointingDirection * SpeedVector;
                Base.Location += movement;
            }
        }

        private int GetTileIndex()
        {
            return (int)Base.Orientation.Name * 4 + animationIndex.CurrentValue;
        }

        private void ChangeDirection()
        {
            Base.Orientation = ObjectOrientation.Random(random);
            directionChangeCountdown = Counter.NewStarted(random.Next(50, 350));
        }

        public void Deserialize(Stream stream)
        {
            animationCountdown = stream.ReadStruct<Counter>();
            animationIndex = stream.ReadStruct<Counter>();
            directionChangeCountdown = stream.ReadStruct<Counter>();
        }

        public void Serialize(Stream stream)
        {
            Base.Serialize(stream);
            stream.WriteStruct(animationCountdown);
            stream.WriteStruct(animationIndex);
            stream.WriteStruct(directionChangeCountdown);
        }
    }
}
