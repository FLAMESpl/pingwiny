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
        public ObjectsCollection Objects { get; private set; } = new ObjectsCollection(false);
        public PenguinGenerator PenguinGenerator { get; private set; } = new PenguinGenerator();
        public PenguinsController PenguinsController { get; private set; } = new PenguinsController();

        public void ClearObjects()
        {
            Objects = new ObjectsCollection(false);
        }

        public void Initialize(GameEnvironment environment, GameState gameState)
        {
            Objects.Initialize(environment, gameState);
            PenguinGenerator.Initialize(environment, gameState);
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Objects = (ObjectsCollection)serializer.ReadNext(stream);
            PenguinGenerator = (PenguinGenerator)serializer.ReadNext(stream);
            PenguinsController = (PenguinsController)serializer.ReadNext(stream);
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            serializer.WriteNext(stream, Objects);
            serializer.WriteNext(stream, PenguinGenerator);
            serializer.WriteNext(stream, PenguinsController);
        }
    }
}
