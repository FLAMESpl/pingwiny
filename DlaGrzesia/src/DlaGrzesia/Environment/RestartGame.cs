using DlaGrzesia.Serialization;

namespace DlaGrzesia.Environment
{
    public class RestartGame : ICommand
    {
        public void Execute(GameEnvironment environment, GameState gameState)
        {
            gameState.Load(new GameState());
            gameState.Stage.Reset(environment, gameState);
        }
    }
}
