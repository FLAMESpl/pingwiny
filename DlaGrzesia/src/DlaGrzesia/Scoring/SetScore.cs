using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;

namespace DlaGrzesia.Scoring
{
    public class SetScore : ICommand
    {
        private readonly int amount;

        public SetScore(int amount)
        {
            this.amount = amount;
        }

        public void Execute(GameEnvironment environment, GameState gameState)
        {
            gameState.Score.Set(amount);
        }
    }
}
