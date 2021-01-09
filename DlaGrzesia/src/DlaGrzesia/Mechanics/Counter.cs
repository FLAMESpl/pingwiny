using System;

namespace DlaGrzesia.Mechanics
{
    public struct Counter
    {
        private Counter(int startValue, int currentValue, int laps, bool cyclic, int tickRate)
        {
            StartValue = startValue;
            CurrentValue = currentValue;
            Laps = laps;
            Cyclic = cyclic;
            TickRate = tickRate;
        }

        public int CurrentValue { get; private set; }
        public int CurrentValueReversed => StartValue - CurrentValue;
        public bool Cyclic { get; private set; }
        public int Laps { get; private set; }
        public int StartValue { get; private set; }
        public int TickRate { get; private set; }

        public bool Elapsed => Laps > 0;

        public Counter ToCyclic()
        {
            Cyclic = true;
            return this;
        }

        public Counter Tick()
        {
            if (Elapsed && Cyclic)
            {
                Laps = 0;
            }

            if (!Elapsed)
            {
                var nextValue = CurrentValue - TickRate;

                while (nextValue < 1)
                {
                    Laps++;
                    nextValue += StartValue;
                }

                CurrentValue = nextValue;
            }

            return this;
        }

        public Counter Reset()
        {
            CurrentValue = StartValue;
            Laps = 0;
            return this;
        }

        public Counter StartFrom(int startValue)
        {
            StartValue = startValue;
            return this;
        }

        public Counter IncreaseTickRate(int rate) => WithTickRate(TickRate + rate);

        public Counter WithTickRate(int rate)
        {
            if (rate < 1) throw new ArgumentException("Cannot be negative", nameof(rate));

            TickRate = rate;
            return this;
        }

        public override string ToString() => CurrentValue.ToString();

        public static Counter NewStarted(int value) => new Counter(value, value, 0, false, 1);
        public static Counter NewElapsed() => NewElapsed(0);
        public static Counter NewElapsed(int maxValue) => new Counter(maxValue, 0, 1, false, 1);
    }
}
