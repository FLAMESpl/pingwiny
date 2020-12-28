using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class PenguinGenerator : IGenerator, IObject, ISerializable
    {
        private readonly Textures textures;
        private readonly Fonts fonts;
        private Counter spawnCooldown = Counter.NewStarted(30).ToCyclic();
        private readonly Random random = new Random();
        private Counter surfingSpawnCooldown = Counter.NewStarted(1_000).ToCyclic();

        public PenguinGenerator(Textures textures, Fonts fonts)
        {
            this.textures = textures;
            this.fonts = fonts;
        }

        public bool Expired => false;

        public Queue<IObject> SpawnedObjects { get; private set; } = new Queue<IObject>();

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            if (spawnCooldown.Elapsed)
            {
                if (random.Next(0, 2) == 0)
                    SpawnSliding(environmentState);
                else
                    SpawnWalking(environmentState);
            }

            if (surfingSpawnCooldown.Elapsed)
                SpawnSurfing(environmentState);

            spawnCooldown = spawnCooldown.Tick();
            surfingSpawnCooldown = surfingSpawnCooldown.Tick();
        }

        private void SpawnSliding(EnvironmentState environment)
        {
            var size = textures.PenguinSliding.TileSize;
            var horizontalPosition = random.Next(environment.StageBounds.Left, environment.StageBounds.Right - size.X + 1);
            var verticalPosition = environment.StageBounds.Top - size.Y + 1;

            var penguin = new SlidingPenguin(
                new PenguinBase(
                    textures.PenguinSliding, 
                    fonts.Font,
                    new ObjectOrientation(ObjectOrientationName.Down),
                    new Point(horizontalPosition, verticalPosition),
                    10,
                    2,
                    20));

            SpawnedObjects.Enqueue(penguin);
        }

        private void SpawnSurfing(EnvironmentState environment)
        {
            var size = textures.PenguinWalking.TileSize;
            var horizontalPosition = random.Next(environment.StageBounds.Left + 100, environment.StageBounds.Right - size.X + 100);
            var verticalPosition = environment.StageBounds.Bottom - 100;

            var penguin = new SurfingPenguin(
                new PenguinBase(
                    textures.PenguinWithBoard,
                    fonts.Font,
                    default,
                    new Point(horizontalPosition, verticalPosition),
                    int.MaxValue,
                    20,
                    0));

            SpawnedObjects.Enqueue(penguin);
        }

        private void SpawnWalking(EnvironmentState environment)
        {
            var size = textures.PenguinWalking.TileSize;
            var horizontalPosition = random.Next(environment.StageBounds.Left, environment.StageBounds.Right - size.X);
            var verticalPosition = random.Next(environment.StageBounds.Top, environment.StageBounds.Bottom - size.Y);

            var penguin = new WalkingPenguin(
                new PenguinBase(
                    textures.PenguinWalking, 
                    fonts.Font, 
                    ObjectOrientation.Random(random),
                    new Point(horizontalPosition, verticalPosition),
                    30,
                    1,
                    5));

            SpawnedObjects.Enqueue(penguin);
        }

        public void Serialize(Stream stream)
        {
            stream.WriteStruct(spawnCooldown);
            stream.WriteStruct(surfingSpawnCooldown);
        }

        public void Deserialize(Stream stream)
        {
            spawnCooldown = stream.ReadStruct<Counter>();
            surfingSpawnCooldown = stream.ReadStruct<Counter>();
        }
    }
}
