using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades.Actions
{
    public class UpgradeSurfingFrequency : IUpgradeAction
    {
        private readonly int levels;

        public UpgradeSurfingFrequency(int levels)
        {
            this.levels = levels;
        }

        public void Execute(GameState gameState, GameEnvironment environment)
        {
            gameState.Stage.PenguinGenerator.DecreaseSurfingCooldown(0.05f * levels);
        }
    }
}
