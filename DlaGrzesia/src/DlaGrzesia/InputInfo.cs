using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia
{
    public class InputInfo
    {
        private KeyboardState? previousKeyboard;
        private MouseState? previousMouse;

        public KeyboardState Keyboard { get; }
        public MouseState Mouse { get; }
        public bool IsMouseClickConsumed { get; private set; }
        public IReadOnlyCollection<Keys> JustPressedKeys { get; private set; }

        public InputInfo(KeyboardState keyboard, MouseState mouse)
        {
            Keyboard = keyboard;
            Mouse = mouse;
            JustPressedKeys = Array.Empty<Keys>();
        }

        public InputInfo GetNext(KeyboardState keyboard, MouseState mouse)
        {
            return new InputInfo(keyboard, mouse)
            {
                previousKeyboard = Keyboard,
                previousMouse = Mouse,
                JustPressedKeys = GetNextJustPressedKeys(keyboard)
            };
        }

        private IReadOnlyCollection<Keys> GetNextJustPressedKeys(KeyboardState nextKeyboard)
        {
            var previouslyPressedKeys = Keyboard.GetPressedKeys();
            return nextKeyboard.GetPressedKeys().Where(x => previouslyPressedKeys.Contains(x) == false).ToArray();
        }

        public bool IsKeyJustPressed(Keys key) => previousKeyboard?.IsKeyUp(key) == true && Keyboard.IsKeyDown(key);
        public bool IsMouseLeftButtonJustPressed() => IsMouseButtonClicked(previousMouse?.LeftButton, Mouse.LeftButton);
        public bool IsMouseRightButtonJustPressed() => IsMouseButtonClicked(previousMouse?.RightButton, Mouse.RightButton);

        public bool TryConsumeLeftMouseButtonClick(Rectangle bounds)
        {
            if (IsMouseLeftButtonJustPressed() && !IsMouseClickConsumed && bounds.Contains(Mouse.Position))
            {
                ConsumeMouseClick();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ConsumeMouseClick() => IsMouseClickConsumed = true;

        private static bool IsMouseButtonClicked(ButtonState? previous, ButtonState next) =>
            previous == ButtonState.Released && next == ButtonState.Pressed;
    }
}
