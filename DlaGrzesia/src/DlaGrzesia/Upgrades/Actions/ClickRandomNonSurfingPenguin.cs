using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using System.Linq;

namespace DlaGrzesia.Upgrades.Actions
{
    public class ClickRandomNonSurfingPenguin : IUpgradeAction
    {
        public void Execute(GameState gameState)
        {
            var penguin = (PenguinBase)gameState.Stage.Objects.FirstOrDefault(static obj =>
            {
                var type = obj.GetType();
                return type.IsAssignableTo(typeof(PenguinBase)) && !type.IsAssignableTo(typeof(SurfingPenguin));
            });

            penguin?.Click();
        }
    }
}
