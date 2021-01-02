using DlaGrzesia.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia.Objects.UI
{
    public class DebugOverlay : GameObject
    {
        private SpriteFont font;

        private readonly CyclicList<IReadOnlyList<Keys>> pressedKeys = 
            new CyclicList<IReadOnlyList<Keys>>(12, Array.Empty<Keys>());

        protected override void OnInitialized()
        {
            font = Environment.Resources.Fonts.Standard;
            base.OnInitialized();
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            if (Environment.IsDebugDataOn)
            {
                var stageBounds = GameState.Stage.Bounds;
                var fps = Math.Round(1000d / gameTime.ElapsedGameTime.TotalMilliseconds, 0);

                batch.DrawStringLines(font, stageBounds.Location,
                    (GameState.Stage.Objects.Count.ToString(), Color.Red),
                    (fps.ToString(), Color.DarkCyan));

                var scale = 0.5f;
                var glyphs = font.GetGlyphs();
                float keyY = stageBounds.Top + font.LineSpacing * 3;
                var color = Color.DarkRed;

                foreach (var keyFrame in pressedKeys.ReadAll().Where(static k => k.Any()))
                {
                    float previousKeyX = stageBounds.Left + 5;
                    keyY += font.LineSpacing * scale;

                    foreach (var key in keyFrame)
                    {
                        var text = key.ToString();
                        batch.DrawString(font, text, new Vector2(previousKeyX, keyY), color, 0, default, scale, default, default);
                        color = color == Color.DarkRed ? Color.Black : Color.DarkRed;
                        previousKeyX += text.Sum(ch => glyphs[ch].WidthIncludingBearings * scale) + 10;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Environment.Input.JustPressedKeys.Any())
                pressedKeys.Write(Environment.Input.JustPressedKeys);
        }
    }
}
