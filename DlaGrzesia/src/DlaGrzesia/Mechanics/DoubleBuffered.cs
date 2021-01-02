namespace DlaGrzesia.Mechanics
{
    public class DoubleBuffered<T>
    {
        private int currentIndex = 0;
        private readonly T[] buffers = new T[2];

        public T Current => buffers[currentIndex];
        public T Other => buffers[OtherIndex];

        private int OtherIndex => currentIndex == 0 ? 1 : 0;

        public DoubleBuffered(T current, T other)
        {
            buffers[currentIndex] = current;
            buffers[OtherIndex] = other;
        }

        public void Swap()
        {
            currentIndex = OtherIndex;
        }
    }

    public static class DoubleBuffered
    {
        public static DoubleBuffered<T> Create<T>() where T : class, new()
        {
            return new DoubleBuffered<T>(new T(), new T());
        }
    }
}
