using DlaGrzesia.Assets;
using DlaGrzesia.Objects.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace DlaGrzesia.Objects.UI
{
    public class PenguinStatsDisplay : GameObject
    {
        private const int ICONS_COUNT = 3;
        private readonly Rectangle boundary;
        private Point[] positions;
        private Tileset icons;
        private SpriteFont font;

        public PenguinStatsDisplay(Point location)
        {
            boundary = new Rectangle(location, new Point(150, 150));
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            var generator = GameState.Stage.PenguinGenerator;
            Span<PenguinStats> stats = stackalloc PenguinStats[3];
            stats[0] = generator.WalkingPenguin;
            stats[1] = generator.SlidingPenguin;
            stats[2] = generator.SurfingPenguin;

            for (int i = 0; i < ICONS_COUNT; i++)
                DrawIconWithStats(i, batch, stats);
        }

        protected override void OnInitialized()
        {
            icons = Environment.Resources.Textures.PenguinIcons;
            font = Environment.Resources.Fonts.Standard;

            var totalIconsHeight = icons.TileSize.Y * ICONS_COUNT;
            var remainingHeight = boundary.Height - totalIconsHeight;
            var topMargin = remainingHeight / (ICONS_COUNT + 1);

            Point GetPositionAtIndex(int index) => 
                boundary.Location + new Point(topMargin, topMargin * index + icons.TileSize.Y * (index - 1));

            positions = Enumerable.Range(1, ICONS_COUNT).Select(GetPositionAtIndex).ToArray();
        }

        private void DrawIconWithStats(int index, SpriteBatch batch, ReadOnlySpan<PenguinStats> penguinStats)
        {
            var stats = penguinStats[index];
            var statsText = stats.PointsPerClick.ToString();

            if (stats.CanBeDestroyed)
                statsText = $"{statsText} - {stats.PointsPerDestroy} ({stats.Duration})";

            batch.Draw(icons, positions[index], index, LayerDepths.UI);
            batch.DrawString(
                font,
                statsText,
                positions[index].ToVector2() + new Vector2(icons.TileSize.X + 10, 0),
                Color.Black,
                0,
                Vector2.Zero,
                0.5f,
                SpriteEffects.None,
                0);
        }
    }
}
