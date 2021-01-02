using DlaGrzesia.Objects;
using DlaGrzesia.Serialization;
using System.Collections.Generic;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class UpgradesCollection : GameObject, ISerializableGameState
    {
        private readonly List<Upgrade> upgrades;

        public Upgrade this[int index] => upgrades[index];

        public UpgradesCollection()
        {
            upgrades = new List<Upgrade>
            {
                new Upgrade(1),
                new Upgrade(10),
                new Upgrade(50),
                new Upgrade(100),
                new Upgrade(250),
                new Upgrade(500),
                new Upgrade(1000),
                new Upgrade(2000)
            };
        }

        public override void Deserialize(Stream stream, GameStateSerializer serializer)
        {
            var count = stream.ReadInt();
            for (int i = 0; i < count; i++)
            {
                upgrades[i] = (Upgrade)serializer.ReadNext(stream);
            }
            base.Deserialize(stream, serializer);
        }

        public override void Serialize(Stream stream, GameStateSerializer serializer)
        {
            stream.WriteInt(upgrades.Count);
            upgrades.ForEach(u => serializer.WriteNext(stream, u));
            base.Serialize(stream, serializer);
        }
    }
}
