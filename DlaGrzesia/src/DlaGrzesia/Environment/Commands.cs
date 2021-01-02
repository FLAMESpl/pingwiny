using DlaGrzesia.Serialization;
using System.Collections.Generic;

namespace DlaGrzesia.Environment
{
    public interface ICommand
    {
        void Execute(GameEnvironment environment, GameState gameState);
    }

    public class Commands
    {
        private readonly List<ICommand> commands = new List<ICommand>();

        public void Add(ICommand command) => commands.Add(command);

        public void ExecuteAll(GameEnvironment environment, GameState state)
        {
            commands.ForEach(cmd => cmd.Execute(environment, state));
            commands.Clear();
        }
    }
}
