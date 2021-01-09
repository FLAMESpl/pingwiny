using DlaGrzesia.Environment;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects;
using DlaGrzesia.Objects.Actors;
using DlaGrzesia.Serialization;
using System;
using System.Linq;

namespace DlaGrzesia.Upgrades.Actions
{
    public class ClickRandomNonSurfingPenguin : IUpgradeAction
    {
        private readonly int count;

        public ClickRandomNonSurfingPenguin(int count)
        {
            if (count == 0) throw new ArgumentException("Cannot be zero", nameof(count));
            this.count = count;
        }

        public void Execute(GameState gameState, GameEnvironment environment)
        {
            Span<GameObject> penguins = gameState.Stage.Objects
                .Where(static obj =>
                {
                    var type = obj.GetType();
                    return type.IsAssignableTo(typeof(PenguinBase)) && !type.IsAssignableTo(typeof(SurfingPenguin));
                })
                .ToArray();

            environment.Random.Shuffle(ref penguins);

            for (int i = 0; i < count && i < penguins.Length; i++)
                ((PenguinBase)penguins[i]).Click();
        }
    }
}
