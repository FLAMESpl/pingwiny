using DlaGrzesia.Serialization;

namespace DlaGrzesia.Environment
{
    public class ToggleDebugData : ICommand
    {
        public void Execute(GameEnvironment environment, GameState gameState)
        {
            environment.SwitchDebugData();
        }
    }
}
