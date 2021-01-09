using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class PenguinsController : ISerializable
    {
        private bool bonusClickActive = false;
        private int bonusClicks;
        private decimal scoreBonus;

        public void ActivateBonusClick(int bonusClicks, decimal scoreBonus)
        {
            this.bonusClicks = bonusClicks;
            this.scoreBonus = scoreBonus;
            bonusClickActive = true;
        }

        public bool TryConsumeBonusClick(Events events, out int bonusClicks, out decimal scoreBonus)
        {
            bonusClicks = this.bonusClicks;
            scoreBonus = this.scoreBonus;

            if (bonusClickActive)
            {
                bonusClickActive = false;
                events.Add(new BonusClickConsumed());
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            bonusClickActive = stream.ReadBool();
            bonusClicks = stream.ReadInt();
            scoreBonus = stream.ReadStruct<decimal>();
        }

        public void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteBool(bonusClickActive);
            stream.WriteInt(bonusClicks);
            stream.WriteStruct(scoreBonus);
        }
    }
}
