using DlaGrzesia.Environment;
using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using System.Linq;

namespace DlaGrzesia.Upgrades.Actions
{
    public class ClickSurfingPenguin : IUpgradeAction
    {
        private readonly int count;

        public ClickSurfingPenguin(int count)
        {
            this.count = count;
        }

        public void Execute(GameState gameState, GameEnvironment environemnt)
        {
            var penguin = gameState.Stage.Objects.OfType<SurfingPenguin>().ToList();

            for (int i = 0; i < penguin.Count && i < count; i++)
                penguin[i].Click();
        }
    }
}
