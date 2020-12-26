using DlaGrzesia.Objects;
using System.Collections.Generic;

namespace DlaGrzesia.Serialization
{
    public class GameState
    {
        public GameState(IReadOnlyCollection<ISerializable> objects, int totalScore)
        {
            Objects = objects;
            TotalScore = totalScore;
        }

        public IReadOnlyCollection<ISerializable> Objects { get; }
        public int TotalScore { get; }
    }
}
