using DlaGrzesia.Environment;
using DlaGrzesia.Objects;
using DlaGrzesia.Scoring;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace DlaGrzesia
{
    public class ScoreDebugInputActivated : IEvent 
    {
        public ScoreDebugInputActivated(string amount)
        {
            Amount = amount;
        }

        public string Amount { get; }
    }

    public class ScoreDebugInputDeactivated : IEvent { }

    public class ScoreDebugInputAmountChanged : IEvent
    {
        public ScoreDebugInputAmountChanged(string amount)
        {
            Amount = amount;
        }

        public string Amount { get; }
    }

    public class MoneyDebugInput : GameObject
    {
        public bool Active { get; private set; } = false;
        public string CurrentMoneyString { get; private set; } = "";

        public override void Update(GameTime gameTime)
        {
            if (Environment.Input.CanHandleKeyboardKeys)
            {
                var keyboard = Environment.Input.Keyboard;
                var keys = Environment.Input.JustPressedKeys;

                if (Active)
                {
                    if (keys.Contains(Keys.Enter))
                    {
                        Accept();
                        Environment.Input.HandleKeyboardKeys();
                    }
                    else if (keys.Contains(Keys.Back))
                    {
                        RemoveCharacter();
                        Environment.Input.HandleKeyboardKeys();
                    }
                    else if (keys.Contains(Keys.M) && IsControlDown(keyboard))
                    {
                        AcceptPredefined();
                        Environment.Input.HandleKeyboardKeys();
                    }
                    else if (TryEnterDigit(keys.FirstOrDefault()))
                    {
                        Environment.Input.HandleKeyboardKeys();
                    }
                }
                else if (IsControlDown(keyboard) && keys.Contains(Keys.M))
                {
                    Activate();
                    Environment.Input.HandleKeyboardKeys();
                }
            }
        }

        private void Activate()
        {
            if (!Active)
            {
                Active = true;
                CurrentMoneyString = GameState.Score.Total.ToString();
                Publish(new ScoreDebugInputActivated(CurrentMoneyString));
            }
        }

        private void Accept()
        {
            if (int.TryParse(CurrentMoneyString, out var money))
                SetScoreAndEndEdit(money);
        }

        private void AcceptPredefined()
        {
            SetScoreAndEndEdit(1_000_000_000);
        }

        private void RemoveCharacter()
        {
            var previousMoneyString = CurrentMoneyString;

            if (CurrentMoneyString.Length > 1)
                CurrentMoneyString = CurrentMoneyString[0..^1];
            else
                CurrentMoneyString = "0";

            PublishChange();
        }

        private bool TryEnterDigit(Keys key)
        {
            var digit = key switch
            {
                Keys.D1 => '1',
                Keys.D2 => '2',
                Keys.D3 => '3',
                Keys.D4 => '4',
                Keys.D5 => '5',
                Keys.D6 => '6',
                Keys.D7 => '7',
                Keys.D8 => '8',
                Keys.D9 => '9',
                Keys.D0 => '0',
                _ => default
            };

            if (digit != default)
            {
                var newMoney = CurrentMoneyString == "0" ? digit.ToString() : CurrentMoneyString + digit;
                if (int.TryParse(newMoney, out _))
                {
                    CurrentMoneyString = newMoney;
                    PublishChange();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetScoreAndEndEdit(int amount)
        {
            Schedule(new SetScore(amount));
            Active = false;
            CurrentMoneyString = string.Empty;
            Publish(new ScoreDebugInputDeactivated());
        }

        private void PublishChange()
        {
            Publish(new ScoreDebugInputAmountChanged(CurrentMoneyString));
        }

        private static bool IsControlDown(KeyboardState keyboard) =>
            keyboard.IsKeyDown(Keys.LeftControl) ||
            keyboard.IsKeyDown(Keys.RightControl);
    }
}
