using DlaGrzesia.Scoring;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace DlaGrzesia
{
    public class MoneyDebugInput
    {
        public bool Active { get; private set; } = false;
        public string CurrentMoneyString { get; private set; } = "";
        public Score NewScore { get; private set; }

        public void Activate(Score current)
        {
            Active = true;
            CurrentMoneyString = current.Total.ToString();
        }

        public void Update(InputInfo inputInfo)
        {
            if (Active)
            {
                if (inputInfo.IsKeyJustPressed(Keys.Enter))
                {
                    if (int.TryParse(CurrentMoneyString, out var money))
                    {
                        NewScore = new Score(money);
                    }

                    Active = false;
                    CurrentMoneyString = "";
                }
                else if (inputInfo.IsKeyJustPressed(Keys.Back))
                {
                    if (CurrentMoneyString.Length > 1)
                        CurrentMoneyString = CurrentMoneyString[0..^1];
                    else
                        CurrentMoneyString = "0";
                }
                else
                {
                    var digit = inputInfo.JustPressedKeys.FirstOrDefault() switch
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
                        _ => default(char?)
                    };

                    if (digit is char ch)
                    {
                        var newMoney = CurrentMoneyString == "0" ? ch.ToString() : CurrentMoneyString + ch;
                        if (int.TryParse(newMoney, out _))
                            CurrentMoneyString = newMoney;
                    }
                }
            }
            else
            {
                NewScore = null;
            }
        }
    }
}
