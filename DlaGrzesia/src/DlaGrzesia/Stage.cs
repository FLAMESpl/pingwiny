using DlaGrzesia.Environment;
using DlaGrzesia.Objects;
using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using Microsoft.Xna.Framework;
using System.IO;

namespace DlaGrzesia
{
    public class Stage : ISerializableGameState
    {
        public Stage() { }

        public Rectangle Bounds { get; } = new Rectangle(15, 15, 1000, 700);
        public ObjectsCollection Objects { get; private set; } = new ObjectsCollection();

        public void Initialize(GameEnvironment environment, GameState gameState)
        {
            Objects = new ObjectsCollection();
            Objects.Initialize(environment, gameState);
            Objects.Add(new PenguinGenerator());
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Objects = (ObjectsCollection)serializer.ReadNext(stream);
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            serializer.WriteNext(stream, Objects);
        }
    }
}
