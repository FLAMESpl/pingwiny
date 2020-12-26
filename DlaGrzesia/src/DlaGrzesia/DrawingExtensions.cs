﻿using DlaGrzesia.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace DlaGrzesia
{
    internal static class DrawingExtensions
    {
        public static void DrawRotated(
            this SpriteBatch batch, 
            Texture2D texture, 
            Point location, 
            Point size,
            Point index, 
            float rotation)
        {

            var source = new Rectangle(size * index, size);

            batch.Draw(
                texture,
                location.ToVector2(),
                source,
                Color.White,
                rotation,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                default);
        }

        public static void Draw(
            this SpriteBatch batch,
            Tileset tileset,
            Point location,
            int index,
            float layer = 0,
            Color? color = null)
        {
            color ??= Color.White;
            var tile = tileset.GetTile(index);
            batch.Draw(
                tile.Texture, 
                location.ToVector2(), 
                tile.Source, 
                color.Value, 
                0, 
                Vector2.Zero, 
                Vector2.One, 
                SpriteEffects.None, 
                layer);
        }
        public static void DrawCenteredString(
            this SpriteBatch batch,
            SpriteFont font,
            string text,
            Point location,
            Color color,
            float scale = 1,
            float layerDepth = 0)
        {
            var size = font.MeasureString(text) * scale;
            var position = location.ToVector2() - size / 2;

            batch.DrawString(font, text, position, color, 0, default, scale, SpriteEffects.None, layerDepth);
        }

        public static void DrawStringAlignedToLeft(
            this SpriteBatch batch,
            SpriteFont font,
            string text,
            Point location,
            Color color,
            float scale = 1,
            float layerDepth = 0)
        {
            var size = font.MeasureString(text) * scale;
            var position = location.ToVector2() - size * Vector2.UnitX;

            batch.DrawString(font, text, position, color, 0, default, scale, SpriteEffects.None, layerDepth);
        }

        public static void DrawStringCoordinates(
            this SpriteBatch batch,
            SpriteFont font,
            Point location)
        {
            var fontScaling = 0.5f;
            batch.DrawString(
                font,
                location.ToString(),
                location.ToVector2() - Vector2.UnitY * (font.LineSpacing * fontScaling),
                Color.Black,
                0,
                Vector2.Zero,
                fontScaling,
                SpriteEffects.None, 0);
        }

        public static void DrawStringLines(
            this SpriteBatch batch,
            SpriteFont font,
            Point originPosition,
            params (string text, Color color)[] lines)
        {
            foreach (var (text, color, line) in lines.Select((x, n) => (x.text, x.color, n)))
            {
                var position = (originPosition + new Point(0, font.LineSpacing * line)).ToVector2();
                batch.DrawString(font, text, position, color);
            }
        }
    }
}
