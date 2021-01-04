using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades.Actions
{
    public class UpgradeSurfingFrequency : IUpgradeAction
    {
        public static readonly UpgradeSurfingFrequency Instance = new UpgradeSurfingFrequency();

        private UpgradeSurfingFrequency()
        {
        }

        public void Execute(GameState gameState)
        {
            gameState.Stage.PenguinGenerator.DecreaseSurfingCooldown(0.05f);
        }
    }
}
