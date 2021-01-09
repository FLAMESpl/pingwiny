using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;
using System;

namespace DlaGrzesia.Upgrades
{
    public interface IUpgradeAction
    {
        void Execute(GameState gameState, GameEnvironment environment);
    }
}
