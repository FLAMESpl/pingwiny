using DlaGrzesia.Environment;
using DlaGrzesia.Serialization;

namespace DlaGrzesia.Objects
{
    public class SpawnObject : ICommand
    {
        private readonly GameObject objectToSpawn;

        public SpawnObject(GameObject objectToSpawn)
        {
            this.objectToSpawn = objectToSpawn;
        }

        public void Execute(GameEnvironment environment, GameState gameState)
        {
            objectToSpawn.Initialize(environment, gameState);
            gameState.Stage.Objects.Add(objectToSpawn);
        }
    }
}
