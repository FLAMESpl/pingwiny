namespace DlaGrzesia.Scoring
{
    public class Score
    {
        public int Total { get; private set; }
        public int InCurrentFrame { get; private set; }

        private int spentInCurrentFrame = 0;

        public Score(int amount)
        {
            Total = amount;
        }

        public void Increase(int amount) => InCurrentFrame += amount;
        public bool Affordable(int amount) => Total - spentInCurrentFrame - amount >= 0;
        public void Spend(int amount) => spentInCurrentFrame += amount;

        public void Update()
        {
            Total += InCurrentFrame - spentInCurrentFrame;
            InCurrentFrame = 0;
            spentInCurrentFrame = 0;
        }
    }
}
