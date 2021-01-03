using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class SurfingFrequencyUpgrade : Upgrade
    {
        private bool leveledUp = false;

        public SurfingFrequencyUpgrade() : base(50)
        {
        }

        public override IUpgradeAction GetAction()
        {
            if (leveledUp)
            {
                leveledUp = false;
                return UpgradeSurfingFrequency.Instance;
            }
            else
            {
                return NoOperation.Instance;
            } 
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            leveledUp = stream.ReadBool();
            base.Deserialize(stream, serializer);   
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteBool(leveledUp);
            base.Serialize(stream, serializer);
        }

        protected override void OnLeveledUp()
        {
            leveledUp = true;
        }
    }
}
