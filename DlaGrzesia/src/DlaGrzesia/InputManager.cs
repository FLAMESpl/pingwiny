using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DlaGrzesia
{
    public class InputManager
    {
        private InputInfo previousState;

        public InputInfo Update(GameWindow gameWindow)
        {
            var keyboard = Keyboard.GetState();
            var mouse = Mouse.GetState(gameWindow);
            previousState = previousState?.GetNext(keyboard, mouse) ?? new InputInfo(keyboard, mouse);
            return previousState;
        }
    }
}
