using DlaGrzesia.Serialization;

namespace DlaGrzesia.Environment
{
    public class TogglePause : ICommand
    {
        public void Execute(GameEnvironment environment, GameState gameState)
        {
            environment.SwitchPause();
        }
    }
}
