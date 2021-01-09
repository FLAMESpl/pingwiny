using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades.Actions
{
    public class UpgradePenguinDuration : IUpgradeAction
    {
        private readonly int levels;

        public UpgradePenguinDuration(int levels)
        {
            this.levels = levels;
        }

        public void Execute(GameState gameState, GameEnvironment gameEnvironment)
        {
            var generator = gameState.Stage.PenguinGenerator;
            generator.IncreasePenguinsDuration(0.1f * levels);
            generator.IncreasePenguinsDestroyBonus(0.075f * levels);
        }
    }
}
