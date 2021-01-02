using DlaGrzesia.Serialization;

namespace DlaGrzesia.Environment
{
    public class SaveGame : ICommand
    {
        public void Execute(GameEnvironment environment, GameState gameState)
        {
            var repository = new GameStateRepository();
            repository.Save(gameState);
            gameState.Events.Add(new GameSaved());
        }
    }
}
