using System.Collections.Generic;

namespace DlaGrzesia.Objects
{
    public interface IGenerator
    {
        Queue<IObject> SpawnedObjects { get; }
    }
}
