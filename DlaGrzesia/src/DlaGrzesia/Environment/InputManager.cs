using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace DlaGrzesia.Environment
{
    public class InputManager : GameObject
    {
        private const int CONFIRMATION_DURATION = 20;

        private Counter resetConfirmation = Counter.NewElapsed(CONFIRMATION_DURATION);
        private Counter exceptionConfirmation = Counter.NewElapsed(CONFIRMATION_DURATION);

        public override void Update(GameTime gameTime)
        {
            resetConfirmation = resetConfirmation.Tick();
            exceptionConfirmation = exceptionConfirmation.Tick();

            var input = Environment.Input;

            if (input.CanHandleKeyboardKeys && input.IsControlKeyDown())
            {
                var pressedKey = input.JustPressedKeys.FirstOrDefault();
                var command = pressedKey switch
                {
                    Keys.D => new ToggleDebugData(),
                    Keys.P => new TogglePause(),
                    Keys.S => new SaveGame(),
                    Keys.L => new LoadGame(),
                    _ => default(ICommand)
                };

                if (pressedKey == Keys.R)
                {
                    if (resetConfirmation.Elapsed)
                    {
                        resetConfirmation = resetConfirmation.Reset();
                        input.HandleKeyboardKeys();
                    }
                    else
                    {
                        command = new RestartGame();
                    }
                }
                else if (pressedKey == Keys.E)
                {
                    if (exceptionConfirmation.Elapsed)
                    {
                        exceptionConfirmation = exceptionConfirmation.Reset();
                        input.HandleKeyboardKeys();
                    }
                    else
                    {
                        command = new ThrowException();
                    }
                }

                if (command is not null)
                {
                    input.HandleKeyboardKeys();
                    Schedule(command);
                }
            }
        }
    }
}
