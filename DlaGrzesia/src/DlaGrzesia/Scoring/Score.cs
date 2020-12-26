namespace DlaGrzesia.Scoring
{
    public class Score
    {
        public int Total { get; private set; }
        public int InCurrentFrame { get; private set; }

        public Score(int amount)
        {
            Total = amount;
        }

        public void Increase(int amount) => InCurrentFrame += amount;

        public void Update()
        {
            Total += InCurrentFrame;
            InCurrentFrame = 0;
        }
    }
}
