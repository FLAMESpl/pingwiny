using DlaGrzesia.Serialization;
using System;

namespace DlaGrzesia.Environment
{
    public class GameEnvironment
    {
        public Commands Commands { get; } = new Commands();
        public InputInfo Input { get; }
        public Random Random { get; } = new Random();
        public GameResources Resources { get; }
        public bool IsPaused { get; private set; }
        public bool IsDebugDataOn { get; private set; }

        private bool saveGameAtTheEndOfCurrentFrame = false;

        public GameEnvironment(
            InputInfo input,
            GameResources resources,
            bool isPaused,
            bool isDebugDataOn)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Resources = resources ?? throw new ArgumentNullException(nameof(resources));
            IsPaused = isPaused;
            IsDebugDataOn = isDebugDataOn;
        }

        public void ExecuteAllCommands(GameState state) => Commands.ExecuteAll(this, state);
        public void SaveGameAtTheEndOfCurrentFrame() => saveGameAtTheEndOfCurrentFrame = true;
        public void SwitchDebugData() => IsDebugDataOn = !IsDebugDataOn;
        public void SwitchPause() => IsPaused = !IsPaused;

        public void SaveGameIfRequested(GameState gameState)
        {
            if (saveGameAtTheEndOfCurrentFrame)
            {
                saveGameAtTheEndOfCurrentFrame = false;
                var repository = new GameStateRepository();
                repository.Save(gameState);
                gameState.Events.Add(new GameSaved());
            }
        }
    }
}