using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia
{
    public class InputInfo
    {
        private KeyboardState previousKeyboard = new KeyboardState();
        private MouseState previousMouse = new MouseState();

        public KeyboardState Keyboard { get; private set; }
        public MouseState Mouse { get; private set; }
        public bool AreKeyboardKeysHandled { get; private set; }
        public bool IsMouseClickConsumed { get; private set; }
        public IReadOnlyList<Keys> JustPressedKeys { get; private set; }

        public bool CanHandleKeyboardKeys => !AreKeyboardKeysHandled && JustPressedKeys.Count > 0;

        public InputInfo(KeyboardState keyboard, MouseState mouse)
        {
            Keyboard = keyboard;
            Mouse = mouse;
            JustPressedKeys = Array.Empty<Keys>();
        }

        public void Update(KeyboardState keyboard, MouseState mouse)
        {
            previousKeyboard = Keyboard;
            previousMouse = Mouse;
            Keyboard = keyboard;
            Mouse = mouse;
            AreKeyboardKeysHandled = false;
            IsMouseClickConsumed = false;
            JustPressedKeys = GetNextJustPressedKeys();
        }

        private IReadOnlyList<Keys> GetNextJustPressedKeys()
        {
            var previouslyPressedKeys = previousKeyboard.GetPressedKeys();
            return Keyboard.GetPressedKeys().Where(x => previouslyPressedKeys.Contains(x) == false).ToArray();
        }

        public bool IsControlKeyDown() => Keyboard.IsKeyDown(Keys.LeftControl) || Keyboard.IsKeyDown(Keys.RightControl);
        public bool IsKeyJustPressed(Keys key) => previousKeyboard.IsKeyUp(key) && Keyboard.IsKeyDown(key);
        public bool IsMouseLeftButtonJustPressed() => IsMouseButtonClicked(previousMouse.LeftButton, Mouse.LeftButton);
        public bool IsMouseRightButtonJustPressed() => IsMouseButtonClicked(previousMouse.RightButton, Mouse.RightButton);

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
        public void HandleKeyboardKeys() => AreKeyboardKeysHandled = true;

        private static bool IsMouseButtonClicked(ButtonState previous, ButtonState next) =>
            previous == ButtonState.Released && next == ButtonState.Pressed;
    }
}
