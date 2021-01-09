using DlaGrzesia.Environment;
using DlaGrzesia.Scoring;
using DlaGrzesia.Serialization;
using DlaGrzesia.Upgrades.Actions;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class Upgrade : ISerializable
    {
        private Upgrade()
        {
        }

        public Upgrade(int basePrice)
        {
            Level = 0;
            BasePrice = basePrice;
            CurrentPrice = basePrice;
        }

        public int Level { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentPrice { get; private set; }
        public bool Bought => Level > 0;
        public bool IsMaxedOut => Level == 999;

        protected int AwardedLevelsThisTick { get; private set; } = 0;

        public void LevelUp(int amount, Score score)
        {
            var boughtLevels = 0;

            for (var i = 0; i < amount && !IsMaxedOut; i++)
            {
                if (score.TrySpend(CurrentPrice))
                {
                    AwardedLevelsThisTick++;
                    Level++;
                    CurrentPrice++;
                    boughtLevels++;
                }
            }

            OnLeveledUp(boughtLevels);
        }

        public void Notify(IEvents events)
        {
            OnNotified(events);
        }

        public IUpgradeAction Update()
        {
            var action = GetAction();
            AwardedLevelsThisTick = 0;
            return action;
        }

        public virtual void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            Level = stream.ReadInt();
            CurrentPrice = stream.ReadInt();
            AwardedLevelsThisTick = stream.ReadInt();
        }

        public virtual void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(Level);
            stream.WriteInt(CurrentPrice);
            stream.WriteInt(AwardedLevelsThisTick);
        }

        public virtual string GetDebugData() => string.Empty;

        protected virtual void OnLeveledUp(int levelsAwarded)
        {
        }

        protected virtual void OnNotified(IEvents events)
        {
        }

        protected virtual IUpgradeAction GetAction()
        {
            return NoOperation.Instance;
        }
    }
}
