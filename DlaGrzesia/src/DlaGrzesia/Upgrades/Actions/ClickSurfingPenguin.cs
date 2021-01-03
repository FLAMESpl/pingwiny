using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using System.Linq;

namespace DlaGrzesia.Upgrades.Actions
{
    public class ClickSurfingPenguin : IUpgradeAction
    {
        public static readonly ClickSurfingPenguin Instance = new ClickSurfingPenguin();

        private ClickSurfingPenguin() { }

        public void Execute(GameState gameState)
        {
            var penguin = gameState.Stage.Objects.OfType<SurfingPenguin>().FirstOrDefault();
            penguin?.Click();
        }
    }
}
