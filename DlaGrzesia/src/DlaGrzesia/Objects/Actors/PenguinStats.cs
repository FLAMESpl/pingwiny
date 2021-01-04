namespace DlaGrzesia.Objects.Actors
{
    public struct PenguinStats
    {
        public readonly int Duration;
        public readonly int PointsPerClick;
        public readonly int PointsPerDestroy;

        public PenguinStats(int duration, int pointsPerClick, int pointsPerDestroy)
        {
            Duration = duration;
            PointsPerClick = pointsPerClick;
            PointsPerDestroy = pointsPerDestroy;
        }

        public bool CanBeDestroyed => Duration != int.MaxValue;
    }
}
