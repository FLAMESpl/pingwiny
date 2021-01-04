using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades.Actions
{
    public class UpgradePenguinDuration : IUpgradeAction
    {
        public static readonly UpgradePenguinDuration Instance = new UpgradePenguinDuration();

        private UpgradePenguinDuration() { }

        public void Execute(GameState gameState)
        {
            var generator = gameState.Stage.PenguinGenerator;
            generator.IncreasePenguinsDuration(0.1f);
            generator.IncreasePenguinsDestroyBonus(0.075f);
        }
    }
}
