﻿using DlaGrzesia.Serialization;

namespace DlaGrzesia.Environment
{
    public class LoadGame : ICommand
    {
        public void Execute(GameEnvironment environment, GameState gameState)
        {
            var repository = new GameStateRepository();
            if (repository.FileExists)
            {
                gameState.Load(repository.Load());
                gameState.Initialize(environment);
            }
        }
    }
}
