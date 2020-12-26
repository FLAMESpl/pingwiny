using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace DlaGrzesia.Objects.UI
{
    public class KonamiCodeSequence
    {
        private static readonly IReadOnlyList<Keys> correctSequence = new[]
        {
            Keys.Up,
            Keys.Up,
            Keys.Down,
            Keys.Down,
            Keys.Left,
            Keys.Right,
            Keys.Left,
            Keys.Right,
            Keys.B,
            Keys.A
        };

        public int CurrentKeyIndex { get; private set; } = 0;

        public bool Update(Keys pressedKey)
        {
            if (correctSequence[CurrentKeyIndex] == pressedKey)
            {
                CurrentKeyIndex++;
                if (CurrentKeyIndex == correctSequence.Count)
                {
                    CurrentKeyIndex = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                CurrentKeyIndex = correctSequence[0] == pressedKey ? 1 : 0;
                return false;
            }
        }
    }
}
