using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades.Actions
{
    public class NoOperation : IUpgradeAction
    {
        public static readonly NoOperation Instance = new NoOperation();
        private NoOperation() { }

        public void Execute(GameState gameState)
        {
        }
    }
}
