using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class PenguinDurationUpgrade : Upgrade
    {
        private bool levelUpThisTick = false;

        public PenguinDurationUpgrade() : base(100)
        {
        }

        public override IUpgradeAction GetAction()
        {
            if (levelUpThisTick)
            {
                levelUpThisTick = false;
                return UpgradePenguinDuration.Instance;
            }
            else
            {
                return NoOperation.Instance;
            }
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            levelUpThisTick = stream.ReadBool();
            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteBool(levelUpThisTick);
            base.Serialize(stream, serializer);
        }

        protected override void OnLeveledUp()
        {
            levelUpThisTick = true;
        }
    }
}
