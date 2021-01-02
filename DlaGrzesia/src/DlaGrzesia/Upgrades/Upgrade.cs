using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class Upgrade : ISerializable
    {
        private Upgrade()
        {
        }

        public Upgrade(int basePrice)
        {
            Level = 0;
            BasePrice = basePrice;
            CurrentPrice = basePrice;
        }

        public int Level { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentPrice { get; private set; }
        public bool Bought => Level > 0;

        public virtual IUpgradeAction GetAction()
        {
            return NoOperation.Instance;
        }

        public void LevelUp()
        {
            Level++;
            CurrentPrice++;
        }

        public virtual void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Level = stream.ReadInt();
            BasePrice = stream.ReadInt();
            CurrentPrice = stream.ReadInt();
        }

        public virtual void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(Level);
            stream.WriteInt(BasePrice);
            stream.WriteInt(CurrentPrice);
        }
    }
}
