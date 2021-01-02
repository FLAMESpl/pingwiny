using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class WalkingPenguin : PenguinBase
    {
        private readonly int speed = 2;
        private Counter animationCountdown = Counter.NewStarted(15).ToCyclic();
        private Counter animationIndex = Counter.NewStarted(3).ToCyclic();
        private readonly Random random = new Random();
        private Counter directionChangeCountdown = Counter.NewStarted(100);
        private Tileset tileset;

        private Point SpeedVector => new Point(speed);

        protected WalkingPenguin() { }

        public WalkingPenguin(
            ObjectOrientation orientation,
            Point location,
            int duration,
            int scorePerClick,
            int scorePerDestroy) : base(
                orientation,
                location,
                duration,
                scorePerClick,
                scorePerDestroy)
        {
        }

        protected override Tileset Tileset => tileset;
        protected override int TilesetIndex => (int)Orientation.Name * 4 + animationIndex.CurrentValue;

        public override void Update(GameTime elapsed)
        {
            base.Update(elapsed);

            if (IsAlive)
            {
                if (directionChangeCountdown.Elapsed)
                    ChangeDirection();
                else
                    directionChangeCountdown = directionChangeCountdown.Tick();

                animationCountdown = animationCountdown.Tick();
                if (animationCountdown.Elapsed)
                    animationIndex = animationIndex.Tick();

                var movement = Orientation.PointingDirection * SpeedVector;
                Location += movement;
            }
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteStruct(animationCountdown);
            stream.WriteStruct(animationIndex);
            stream.WriteStruct(directionChangeCountdown);
            base.Serialize(stream, serializer);
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            animationCountdown = stream.ReadStruct<Counter>();
            animationIndex = stream.ReadStruct<Counter>();
            directionChangeCountdown = stream.ReadStruct<Counter>();
            base.Deserialize(stream, serializer);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            tileset = Environment.Resources.Textures.PenguinWalking;
        }

        private void ChangeDirection()
        {
            Orientation = ObjectOrientation.Random(random);
            directionChangeCountdown = Counter.NewStarted(random.Next(50, 350));
        }
    }
}
