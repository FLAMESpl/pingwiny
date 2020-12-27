using DlaGrzesia.Serialization;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class UpgradeState : ISerializable
    {
        private int basePrice;
        private int currentPrice;
        private int count = 0;

        public UpgradeState(int basePrice, int count)
        {
            this.basePrice = basePrice;
            this.count = count;
            currentPrice = GetPriceForCurrentCount();
        }

        public Upgrade ToFrameState() => new Upgrade(currentPrice, count);

        public void LevelUp()
        {
            count++;
            currentPrice = GetPriceForCurrentCount();
        }

        public void Serialize(Stream stream)
        {
            stream.WriteInt(basePrice);
            stream.WriteInt(count);
        }

        public void Deserialize(Stream stream)
        {
            basePrice = stream.ReadInt();
            count = stream.ReadInt();
            currentPrice = GetPriceForCurrentCount();
        }

        private int GetPriceForCurrentCount() => count + basePrice;
    }
}
