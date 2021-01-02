using DlaGrzesia.Serialization;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class Upgrade : ISerializableGameState
    {
        private Upgrade()
        {
        }

        public Upgrade(int basePrice) : this(0, basePrice, basePrice)
        {
        }

        public Upgrade(int level, int basePrice, int currentPrice)
        {
            Level = level;
            BasePrice = basePrice;
            CurrentPrice = currentPrice;
        }

        public int Level { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentPrice { get; private set; }

        public void LevelUp()
        {
            Level++;
            CurrentPrice++;
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Level = stream.ReadInt();
            BasePrice = stream.ReadInt();
            CurrentPrice = stream.ReadInt();
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(Level);
            stream.WriteInt(BasePrice);
            stream.WriteInt(CurrentPrice);
        }
    }
}
