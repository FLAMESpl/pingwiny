using System.Collections.Generic;

namespace DlaGrzesia
{
    public struct Upgrade
    {
        public readonly int Price;
        public readonly int Count;

        public Upgrade(int price, int count)
        {
            Price = price;
            Count = count;
        }
    }

    public class Upgrades
    {
        private readonly IReadOnlyList<Upgrade> upgrades;
        private readonly List<int> boughtUpgrades = new List<int>();

        public Upgrades(IReadOnlyList<Upgrade> upgrades)
        {
            this.upgrades = upgrades;
        }

        public Upgrade this[int index] => upgrades[index];
        public IReadOnlyList<int> BoughtUpgrades => boughtUpgrades;

        public void Buy(int index)
        {
            boughtUpgrades.Add(index);
        }
    }
}
