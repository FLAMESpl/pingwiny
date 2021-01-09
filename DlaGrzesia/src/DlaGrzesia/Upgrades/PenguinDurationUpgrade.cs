using DlaGrzesia.Upgrades.Actions;

namespace DlaGrzesia.Upgrades
{
    public class PenguinDurationUpgrade : Upgrade
    {
        public PenguinDurationUpgrade() : base(100)
        {
        }

        protected override IUpgradeAction GetAction()
        {
            if (AwardedLevelsThisTick > 0)
            {
                return new UpgradePenguinDuration(AwardedLevelsThisTick);
            }
            else
            {
                return NoOperation.Instance;
            }
        }
    }
}
