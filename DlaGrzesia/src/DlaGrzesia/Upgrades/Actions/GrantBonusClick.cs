using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades.Actions
{
    public class GrantBonusClick : IUpgradeAction
    {
        private readonly int bonusClicks;
        private readonly decimal bonusScore;

        public GrantBonusClick(int bonusClicks, decimal bonusScore)
        {
            this.bonusClicks = bonusClicks;
            this.bonusScore = bonusScore;
        }

        public void Execute(GameState gameState, GameEnvironment environment)
        {
            gameState.Stage.PenguinsController.ActivateBonusClick(bonusClicks, bonusScore);
        }
    }
}
