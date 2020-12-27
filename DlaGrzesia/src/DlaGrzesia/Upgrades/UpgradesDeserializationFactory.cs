using DlaGrzesia.Serialization;
using System.IO;

namespace DlaGrzesia.Upgrades
{
    public class UpgradesDeserializationFactory : IDeserializerFactory
    {
        public string Type { get; } = typeof(UpgradeState).FullName;

        public ISerializable CreateAndDeserialize(Stream stream)
        {
            var upgradeState = new UpgradeState(default, default);
            upgradeState.Deserialize(stream);
            return upgradeState;
        }
    }
}
