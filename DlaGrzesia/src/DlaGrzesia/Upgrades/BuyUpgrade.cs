using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades
{
    public class BuyUpgrade : ICommand
    {
        private readonly int upgradeIndex;

        public BuyUpgrade(int upgradeIndex)
        {
            this.upgradeIndex = upgradeIndex;
        }

        public void Execute(GameEnvironment environment, GameState gameState)
        {
            var upgrade = gameState.Upgrades[upgradeIndex];
            if (gameState.Score.TrySpend(upgrade.CurrentPrice))
                upgrade.LevelUp();
        }
    }
}
