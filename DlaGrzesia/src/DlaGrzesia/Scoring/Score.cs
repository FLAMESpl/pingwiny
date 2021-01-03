using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using System.IO;
using System.Linq;

namespace DlaGrzesia.Scoring
{
    public class Score : ISerializableGameState
    {
        public int Total { get; private set; }
        public int InCurrentFrame { get; private set; }
        public decimal PerSecond => ((decimal)scoreGains.ReadAll().Sum()) / 60;

        private CyclicList<int> scoreGains = new CyclicList<int>(60);

        public Score()
        {
            Total = 0;
        }

        public Score(int amount)
        {
            Total = amount;
        }

        public void Increase(int amount) => InCurrentFrame += amount;
        public bool Affordable(int amount) => Total - amount >= 0;

        public bool TrySpend(int amount)
        {
            if (Affordable(amount))
            {
                Total -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Set(int amount)
        {
            Total = amount;
            InCurrentFrame = 0;
        }

        public void Update()
        {
            scoreGains.Write(InCurrentFrame);
            Total += InCurrentFrame;
            InCurrentFrame = 0;
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(Total);
            stream.WriteInt(InCurrentFrame);
            scoreGains.Serialize(stream, static (s, i) => s.WriteInt(i));
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Total = stream.ReadInt();
            InCurrentFrame = stream.ReadInt();
            scoreGains.Deserialize(stream, static s => s.ReadInt());
        }
    }
}
