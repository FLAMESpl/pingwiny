using DlaGrzesia.Scoring;
using Microsoft.Xna.Framework;

namespace DlaGrzesia
{
    public class EnvironmentState
    {
        public EnvironmentState(Rectangle stageBounds, InputInfo input, Score score)
        {
            StageBounds = stageBounds;
            Input = input;
            Score = score;
        }

        public Rectangle StageBounds { get; }
        public InputInfo Input { get; }
        public Score Score { get; }
    }
}
