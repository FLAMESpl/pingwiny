using DlaGrzesia.Environment;
using DlaGrzesia.Scoring;
using DlaGrzesia.Upgrades;
using System.IO;

namespace DlaGrzesia.Serialization
{
    public class GameState : ISerializableGameState
    {
        public Events Events { get; private set; } = new Events();
        public Score Score { get; private set; } = new Score();
        public Stage Stage { get; private set; } = new Stage();
        public UpgradesCollection Upgrades { get; private set; } = new UpgradesCollection();

        public void Load(GameState state)
        {
            Events = state.Events;
            Score = state.Score;
            Stage = state.Stage;
            Upgrades = state.Upgrades;
        }

        public void Initialize(GameEnvironment environment)
        {
            Stage.Initialize(environment, this);
            Upgrades.Initialize(environment, this);
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Events = (Events)serializer.ReadNext(stream);
            Score = (Score)serializer.ReadNext(stream);
            Stage = (Stage)serializer.ReadNext(stream);
            Upgrades = (UpgradesCollection)serializer.ReadNext(stream);
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            serializer.WriteNext(stream, Events);
            serializer.WriteNext(stream, Score);
            serializer.WriteNext(stream, Stage);
            serializer.WriteNext(stream, Upgrades);
        }
    }
}
