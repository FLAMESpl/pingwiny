using DlaGrzesia.Assets;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public abstract class PenguinBase : GameObject, ISerializableGameState
    {
        private SpriteFont font;
        private int remainingDuration;
        private bool spawnedParticleThisTick = false;
        private PenguinStats stats;

        protected PenguinBase() { }

        public PenguinBase(
            Point location,
            PenguinStats stats)
        {
            Location = location;
            this.stats = stats;
            remainingDuration = stats.Duration;
        }

        protected Point Location { get; set; }
        protected Rectangle Bounds => new Rectangle(Location, Tileset.TileSize);
        protected abstract Tileset Tileset { get; }
        protected virtual int TilesetIndex => 0;

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            var color = Environment.IsPaused? Color.DarkSlateGray : Color.White;
            batch.Draw(Tileset, Location, TilesetIndex, LayerDepths.Actors, color);
            if (Environment.IsDebugDataOn)
            {
                batch.DrawStringCoordinates(font, Location);
                batch.DrawString(
                    font, 
                    remainingDuration.ToString(), 
                    new Vector2(Bounds.Right, Bounds.Top),
                    Color.Red,
                    default,
                    default,
                    0.5f,
                    SpriteEffects.None,
                    default);
            }
        }

        public void Click()
        {
            remainingDuration--;

            if (remainingDuration == 0)
            {
                Destroy();
                HandleClick(true, stats.PointsPerDestroy);
            }
            else
            {
                HandleClick(false, stats.PointsPerClick);
            }
        }

        public override void Update(GameTime gameTime)
        {
            spawnedParticleThisTick = false;

            if (Environment.Input.TryConsumeLeftMouseButtonClick(Bounds))
                Click();
            
            if (GameState.Stage.Bounds.Intersects(Bounds) == false)
                Destroy();
        }

        protected override void OnInitialized()
        {
            font = Environment.Resources.Fonts.Standard;
        }

        private void HandleClick(bool yellowParticle, int score)
        {
            GameState.Score.Increase(score);
            if (!spawnedParticleThisTick)
            {
                spawnedParticleThisTick = true;
                Schedule(new SpawnObject(new HeartParticle(yellowParticle, Location)));
            }
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(remainingDuration);
            stream.WriteStruct(stats);
            stream.WriteStruct(Location);

            base.Serialize(stream, serializer);
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            remainingDuration = stream.ReadInt();
            stats = stream.ReadStruct<PenguinStats>();
            Location = stream.ReadStruct<Point>();

            base.Deserialize(stream, serializer);
        }
    }
}
