using DlaGrzesia.Upgrades.Actions;

namespace DlaGrzesia.Upgrades
{
    public class SurfingFrequencyUpgrade : Upgrade
    {
        public SurfingFrequencyUpgrade() : base(50)
        {
        }

        protected override IUpgradeAction GetAction()
        {
            if (AwardedLevelsThisTick > 0)
            {
                return new UpgradeSurfingFrequency(AwardedLevelsThisTick);
            }
            else
            {
                return NoOperation.Instance;
            } 
        }
    }
}
