using System.Diagnostics;

namespace DlaGrzesia.Mechanics
{
    public struct Counter
    {
        public Counter(int startValue, int currentValue, bool cyclic)
        {
            StartValue = startValue;
            CurrentValue = currentValue;
            Cyclic = cyclic;
        }

        public int CurrentValue { get; private set; }
        public int CurrentValueReversed => StartValue - CurrentValue;
        public bool Cyclic { get; }
        public int StartValue { get; private set; }

        public bool Elapsed => CurrentValue == 0;

        public Counter ToCyclic()
        {
            return new Counter(StartValue, CurrentValue, true);
        }

        public Counter Tick()
        {
            if (CurrentValue > 0)
                CurrentValue--;
            else if (Cyclic)
                CurrentValue = StartValue;

            return this;
        }

        public Counter Reset()
        {
            CurrentValue = StartValue;
            return this;
        }

        public Counter StartFrom(int startValue)
        {
            StartValue = startValue;
            return this;
        }

        public override string ToString() => CurrentValue.ToString();

        public static Counter NewStarted(int value) => new Counter(value, value, false);
        public static Counter NewElapsed() => new Counter(0, 0, false);
        public static Counter NewElapsed(int maxValue) => new Counter(maxValue, 0, false);
    }
}
