using DlaGrzesia.Environment;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class BonusClickActivator : Upgrade
    {
        private const int BASE_COOLDOWN = 900;
        private const int COOLDOWN_PER_LEVEL = 30;
        private const int BASE_CLICKS = 10;
        private const int CLICKS_PER_LEVEL = 2;
        private const decimal BASE_SCORE_BONUS = 1.5M;
        private const decimal SCORE_BONUS_PER_LEVEL = 0.5M;

        private int clicks;
        private decimal scoreBonus;
        private Counter activationCounter;
        private bool shouldActivate = true;

        public BonusClickActivator() : base(250)
        {
            SetAttributes(1);
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            clicks = stream.ReadInt();
            scoreBonus = stream.ReadStruct<decimal>();
            activationCounter = stream.ReadStruct<Counter>();
            shouldActivate = stream.ReadBool();

            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(clicks);
            stream.WriteStruct(scoreBonus);
            stream.WriteStruct(activationCounter);
            stream.WriteBool(shouldActivate);

            base.Serialize(stream, serializer);
        }

        public override string GetDebugData() => activationCounter.Elapsed
            ? "READY"
            : activationCounter.CurrentValue.ToString();

        protected override IUpgradeAction GetAction()
        {
            activationCounter.Tick();
            if (activationCounter.Elapsed && shouldActivate)
            {
                shouldActivate = false;
                return new GrantBonusClick(clicks, scoreBonus);
            }
            else
            {
                return NoOperation.Instance;
            }
        }

        protected override void OnLeveledUp(int levelsAwarded) => SetAttributes(Level);

        protected override void OnNotified(IEvents events)
        {
            if (events.TryGetFirst<BonusClickConsumed>(out _))
            {
                activationCounter.Reset();
                shouldActivate = true;
            }
        }

        private void SetAttributes(int level)
        {
            clicks = CalculateClicksAmount(level);
            activationCounter = Counter.NewStarted(CalculateCooldown(level));
            scoreBonus = CaclulateScoreBonus(level);
        }

        private static int CalculateClicksAmount(int level) => BASE_CLICKS + CLICKS_PER_LEVEL * level;
        private static int CalculateCooldown(int level) => BASE_COOLDOWN + COOLDOWN_PER_LEVEL * level;
        private static decimal CaclulateScoreBonus(int level) => BASE_SCORE_BONUS + SCORE_BONUS_PER_LEVEL * level;
    }
}
