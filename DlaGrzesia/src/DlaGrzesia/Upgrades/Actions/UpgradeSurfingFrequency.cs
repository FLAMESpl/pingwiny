using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using System.Linq;

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
            var generator = gameState.Stage.Objects.OfType<PenguinGenerator>().First();
            generator.DecreaseSurfingCooldown(0.05f);
        }
    }
}
