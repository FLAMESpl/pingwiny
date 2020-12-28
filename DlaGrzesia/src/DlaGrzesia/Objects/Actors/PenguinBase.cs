using DlaGrzesia.Assets;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class PenguinBase : ISerializable
    {
        private readonly Tileset tileset;
        private readonly SpriteFont font;
        private int remainingDuration;
        private int scorePerClick;
        private int scorePerDestroy;

        public PenguinBase(
            Tileset tileset,
            SpriteFont font,
            ObjectOrientation orientation,
            Point location,
            int duration,
            int scorePerClick,
            int scorePerDestroy)
        {
            this.tileset = tileset;
            this.font = font;
            Orientation = orientation;
            Location = location;
            remainingDuration = duration;
            this.scorePerClick = scorePerClick;
            this.scorePerDestroy = scorePerDestroy;
        }

        public ObjectOrientation Orientation { get; set; }
        public Point Location { get; set; }
        public bool Expired { get; private set; }
        public Rectangle Bounds => new Rectangle(Location, tileset.TileSize);

        public void Draw(SpriteBatch batch, DrawingModifiers modifiers, int tilesetIndex)
        {
            var color = modifiers.IsGamePaused ? Color.DarkSlateGray : Color.White;
            batch.Draw(tileset, Location, tilesetIndex, LayerDepths.Actors, color);
            if (modifiers.IncludeDebugData)
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

        public void Update(EnvironmentState environmentState)
        {
            if (environmentState.Input.TryConsumeLeftMouseButtonClick(Bounds))
            {
                remainingDuration--;

                if (remainingDuration == 0)
                {
                    Expired = true;
                    environmentState.Score.Increase(scorePerDestroy);
                    environmentState.HeartsGenerator.Spawn(new HeartsParticle(true, Location));
                }
                else
                {
                    environmentState.Score.Increase(scorePerClick);
                    environmentState.HeartsGenerator.Spawn(new HeartsParticle(false, Location));
                }
            }
            
            if (environmentState.StageBounds.Intersects(Bounds) == false)
            {
                Expired = true;
            }
        }

        public void Serialize(Stream stream)
        {
            stream.WriteInt(remainingDuration);
            stream.WriteInt(scorePerClick);
            stream.WriteInt(scorePerDestroy);
            stream.WriteBool(Expired);
            stream.WriteStruct(Location);
            stream.WriteStruct(Orientation);
        }

        public void Deserialize(Stream stream)
        {
            remainingDuration = stream.ReadInt();
            scorePerClick = stream.ReadInt();
            scorePerDestroy = stream.ReadInt();
            Expired = stream.ReadBool();
            Location = stream.ReadStruct<Point>();
            Orientation = stream.ReadStruct<ObjectOrientation>();
        }
    }
}
