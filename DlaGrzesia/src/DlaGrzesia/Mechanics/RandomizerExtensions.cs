using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia.Mechanics
{
    internal static class RandomizerExtensions
    {
        public static List<T> Shuffle<T>(this Random random, IEnumerable<T> source)
        {
            var result = source.ToList();
            for (var n = result.Count - 1; n > 0; n--)
            {
                var k = random.Next(n + 1);
                var value = result[k];
                result[k] = result[n];
                result[n] = value;
            }
            return result;
        }

        public static void Shuffle<T>(this Random random, ref Span<T> source)
        {
            for (var n = source.Length - 1; n > 0; n--)
            {
                var k = random.Next(n + 1);
                var value = source[k];
                source[k] = source[n];
                source[n] = value;
            }
        }
    }
}
