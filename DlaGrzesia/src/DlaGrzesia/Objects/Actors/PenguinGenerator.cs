using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class PenguinGenerator : GameObject, ISerializableGameState
    {
        private Counter spawnCooldown = Counter.NewStarted(30).ToCyclic();
        private readonly Random random = new Random();
        private float surfingSpawnCooldownNonRounded = 1_000;
        private Counter surfingSpawnCooldown = Counter.NewStarted(1_000).ToCyclic();
        private Textures textures;

        private Rectangle StageBounds => GameState.Stage.Bounds;

        protected override void OnInitialized()
        {
            textures = Environment.Resources.Textures;
        }

        public override void Update(GameTime elapsed)
        {
            if (spawnCooldown.Elapsed)
            {
                if (random.Next(0, 2) == 0)
                    SpawnSliding();
                else
                    SpawnWalking();
            }

            if (surfingSpawnCooldown.Elapsed)
                SpawnSurfing();

            spawnCooldown = spawnCooldown.Tick();
            surfingSpawnCooldown = surfingSpawnCooldown.Tick();
        }

        public void DecreaseSurfingCooldown(float rate)
        {
            var newStartCandidate = surfingSpawnCooldownNonRounded * (1 - rate);
            var newStartRounded = (int)Math.Round(newStartCandidate);
            int newStart;

            if (newStartRounded > 1)
            {
                surfingSpawnCooldownNonRounded = newStartCandidate;
                newStart = newStartRounded;
            }
            else
            {
                surfingSpawnCooldownNonRounded = newStart = 1;
            }

            surfingSpawnCooldown.StartFrom(newStart);
        }

        private void SpawnSliding()
        {
            var size = textures.PenguinSliding.TileSize;
            var horizontalPosition = random.Next(StageBounds.Left, StageBounds.Right - size.X + 1);
            var verticalPosition = StageBounds.Top - size.Y + 1;

            Spawn(new SlidingPenguin(
                new Point(horizontalPosition, verticalPosition),
                10,
                2,
                20));
        }

        private void SpawnSurfing()
        {
            var size = textures.PenguinWalking.TileSize;
            var horizontalPosition = random.Next(StageBounds.Left + 100, StageBounds.Right - size.X + 100);
            var verticalPosition = StageBounds.Bottom - 100;

            Spawn(new SurfingPenguin(
                new Point(horizontalPosition, verticalPosition),
                int.MaxValue,
                20,
                0));
        }

        private void SpawnWalking()
        {
            var orientation = random.Next(0, 2) == 0
                ? new ObjectOrientation(ObjectOrientationName.Left)
                : new ObjectOrientation(ObjectOrientationName.Right);

            var size = textures.PenguinWalking.TileSize;
            var horizontalPosition = orientation.Name == ObjectOrientationName.Left
                ? StageBounds.Right - 1
                : StageBounds.Left + 1 - size.X;

            var verticalPosition = random.Next(StageBounds.Top, StageBounds.Bottom - size.Y);

            Spawn(new WalkingPenguin(
                orientation,
                new Point(horizontalPosition, verticalPosition),
                30,
                1,
                5));
        }

        private void Spawn(PenguinBase penguin)
        {
            Schedule(new SpawnObject(penguin));
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteStruct(spawnCooldown);
            stream.WriteStruct(surfingSpawnCooldown);
            base.Serialize(stream, serializer);
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            spawnCooldown = stream.ReadStruct<Counter>();
            surfingSpawnCooldown = stream.ReadStruct<Counter>();
            base.Deserialize(stream, serializer);
        }
    }
}
