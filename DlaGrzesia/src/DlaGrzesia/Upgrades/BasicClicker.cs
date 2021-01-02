using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class BasicClicker : Upgrade
    {
        private Counter clickCooldown = Counter.NewStarted(99).ToCyclic();
        private int previousLevel = 0;

        public BasicClicker() : base(1)
        {
        }

        public override IUpgradeAction GetAction()
        {
            clickCooldown.Tick();

            if (clickCooldown.Elapsed)
            {
                if (previousLevel != Level)
                {
                    previousLevel = Level;
                    clickCooldown = Counter.NewStarted(100 - Level).ToCyclic();
                }

                return new ClickRandomNonSurfingPenguin();
            }
            else
            {
                return NoOperation.Instance;
            }
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteStruct(clickCooldown);
            stream.WriteInt(previousLevel);
            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            clickCooldown = stream.ReadStruct<Counter>();
            previousLevel = stream.ReadInt();
            base.Serialize(stream, serializer);
        }
    }
}
