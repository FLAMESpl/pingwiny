using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Scoring;
using Microsoft.Xna.Framework;

namespace DlaGrzesia
{
    public class EnvironmentState
    {
        public EnvironmentState(
            Rectangle stageBounds, 
            InputInfo input, 
            Score score, 
            Events events,
            Upgrades upgrades,
            MoneyDebugInput moneyDebugInput,
            ParticleGenerator heartsGenerator)
        {
            StageBounds = stageBounds;
            Input = input;
            Score = score;
            Events = events;
            Upgrades = upgrades;
            MoneyDebugInput = moneyDebugInput;
            HeartsGenerator = heartsGenerator;
        }

        public Rectangle StageBounds { get; }
        public InputInfo Input { get; }
        public Score Score { get; }
        public Events Events { get; }
        public Upgrades Upgrades { get; }
        public MoneyDebugInput MoneyDebugInput { get; }
        public ParticleGenerator HeartsGenerator { get; }
    }

    public class Events
    {
        public Events(bool gameSaved)
        {
            GameSaved = gameSaved;
        }

        public bool GameSaved { get; }
    }
}
