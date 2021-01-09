using DlaGrzesia.Mechanics;
using Xunit;

namespace DlaGrzesia.Tests
{
    public class CounterTests
    {
        [Fact]
        public void NonCyclicTest()
        {
            var counter = Counter.NewStarted(3);
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldHaveLaps(1);
            counter.Tick();
            counter.ShouldHaveLaps(1);
            counter.Tick();
            counter.ShouldHaveLaps(1);
            counter.Tick();
            counter.ShouldHaveLaps(1);
            counter.Tick();
            counter.ShouldHaveLaps(1);
        }

        [Fact]
        public void CyclicTest()
        {
            var counter = Counter.NewStarted(3).ToCyclic();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldHaveLaps(1);
            counter.Tick();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldNotBeElapsed();
            counter.Tick();
            counter.ShouldHaveLaps(1);
            counter.Tick();
            counter.ShouldNotBeElapsed();
        }
    }
}
