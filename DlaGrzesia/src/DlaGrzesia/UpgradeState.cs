using DlaGrzesia.Serialization;
using System.IO;

namespace DlaGrzesia
{
    public class UpgradeState : ISerializable
    {
        private readonly int basePrice;
        private int currentPrice;
        private int count = 0;

        public UpgradeState(int basePrice)
        {
            this.basePrice = basePrice;
            currentPrice = basePrice;
        }

        public Upgrade ToFrameState() => new Upgrade(currentPrice, count);

        public void LevelUp()
        {
            currentPrice++;
            count++;
        }

        public void Serialize(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public void Deserialize(Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}
