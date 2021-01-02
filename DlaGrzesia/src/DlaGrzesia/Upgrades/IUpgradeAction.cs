using DlaGrzesia.Serialization;

namespace DlaGrzesia.Upgrades
{
    public interface IUpgradeAction
    {
        void Execute(GameState gameState);
    }
}
