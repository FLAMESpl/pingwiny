using DlaGrzesia.Mechanics;
using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class SurfingClicker : Upgrade
    {
        private const int TICK_RATE_INCREMENT = 1;
        private Counter activationCounter = Counter.NewStarted(60).WithTickRate(TICK_RATE_INCREMENT).ToCyclic();

        public SurfingClicker() : base(10)
        {
        }

        protected override IUpgradeAction GetAction()
        {
            activationCounter.Tick();
            return activationCounter.Elapsed
                ? new ClickSurfingPenguin(activationCounter.Laps)
                : NoOperation.Instance;
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            activationCounter = stream.ReadStruct<Counter>();
            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteStruct(activationCounter);
            base.Serialize(stream, serializer);
        }

        public override string GetDebugData() => activationCounter.CurrentValue.ToString();

        protected override void OnLeveledUp(int levels)
        {
            activationCounter.IncreaseTickRate(TICK_RATE_INCREMENT * levels);
        }
    }
}
