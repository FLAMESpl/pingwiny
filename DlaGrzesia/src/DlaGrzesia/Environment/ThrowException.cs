using DlaGrzesia.Serialization;
using System;

namespace DlaGrzesia.Environment
{
    public class ThrowException : ICommand
    {
        public class TestException : Exception
        {
            public TestException() : base("Test exception")
            {
            }
        }

        public void Execute(GameEnvironment environment, GameState gameState)
        {
            throw new TestException();
        }
    }
}
