using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class SurfingPenguin : PenguinBase
    {
        private Counter ascending = Counter.NewStarted(24);
        private readonly int horizontalSpeed = 4;
        private readonly int verticalSpeed = 3;
        private Tileset tileset;

        protected SurfingPenguin() { }

        public SurfingPenguin(
            Point location,
            PenguinStats stats) : base(
                location,
                stats)
        {
        }

        protected override Tileset Tileset => tileset;

        public override void Update(GameTime elapsed)
        {
            base.Update(elapsed);

            if (IsAlive)
            {
                Location += GetMovement();
                ascending = ascending.Tick();
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            tileset = Environment.Resources.Textures.PenguinWithBoard;
        }

        private Point GetMovement()
        {
            var direction = ascending.Elapsed ? 1 : -1;
            return new Point(-horizontalSpeed, verticalSpeed * direction);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteStruct(ascending);
            base.Serialize(stream, serializer);
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            ascending = stream.ReadStruct<Counter>();
            base.Deserialize(stream, serializer);
        }
    }
}
