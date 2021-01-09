using DlaGrzesia.Mechanics;
using FluentAssertions;

namespace DlaGrzesia.Tests
{
    public static class CounterAssertions
    {
        public static void ShouldHaveLaps(this Counter counter, int count)
        {
            counter.Laps.Should().Be(count);
            counter.Elapsed.Should().BeTrue();
        }

        public static void ShouldNotBeElapsed(this Counter counter)
        {
            counter.Laps.Should().Be(0);
            counter.Elapsed.Should().BeFalse();
        }
    }
}
