using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Scoring;
using DlaGrzesia.Upgrades;
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
            UpgradesCollection upgrades,
            MoneyDebugInput moneyDebugInput,
            ParticleGenerator heartsGenerator,
            bool isGamePaused)
        {
            StageBounds = stageBounds;
            Input = input;
            Score = score;
            Events = events;
            Upgrades = upgrades;
            MoneyDebugInput = moneyDebugInput;
            HeartsGenerator = heartsGenerator;
            IsGamePaused = isGamePaused;
        }

        public Rectangle StageBounds { get; }
        public InputInfo Input { get; }
        public Score Score { get; }
        public Events Events { get; }
        public UpgradesCollection Upgrades { get; }
        public MoneyDebugInput MoneyDebugInput { get; }
        public ParticleGenerator HeartsGenerator { get; }
        public bool IsGamePaused { get; }
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
