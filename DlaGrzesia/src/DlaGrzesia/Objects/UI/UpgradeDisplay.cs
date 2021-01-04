using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.UI
{
    public class UpgradeDisplay : GameObject
    {
        private const int CTRL_LEVELS_COUNT = 10;
        private const int SHIFT_LEVELS_COUNT = 100;
        private SpriteFont font;
        private readonly Rectangle bounds;
        private readonly int index;
        private bool canAfford = false;
        private Counter justBoughtDuration = Counter.NewElapsed(48);

        public UpgradeDisplay(Rectangle bounds, int index)
        {
            this.bounds = bounds;
            this.index = index;
        }

        private Point PriceLocation => new Point(bounds.Right - 10, bounds.Bottom - 40);
        private Point CountLocation => new Point(bounds.Center.X + 75, bounds.Center.Y);
        private Upgrade Upgrade => GameState.Upgrades[index];
        private Tileset Tileset => GameState.Upgrades.GetTileset(index);

        public override void Draw(GameTime elapsed, SpriteBatch spriteBatch)
        {
            var priceColor = canAfford ? Color.Red : Color.DarkGray;

            if (Upgrade.Level == 0)
            {
                spriteBatch.DrawCenteredString(font, Upgrade.CurrentPrice.ToString(), bounds.Center, priceColor, 1f, LayerDepths.UI);
            }
            else
            {
                var tileIndex = Environment.IsPaused ? 2 : justBoughtDuration.Elapsed ? 0 : 1;
                spriteBatch.Draw(Tileset, bounds.Location, tileIndex, LayerDepths.UI);
                spriteBatch.DrawStringAlignedToLeft(font, Upgrade.CurrentPrice.ToString(), PriceLocation, priceColor, 0.5f, LayerDepths.UI);
                spriteBatch.DrawCenteredString(font, Upgrade.Level.ToString(), CountLocation, Color.Black, 1, LayerDepths.UI);
            }
        }

        public override void Update(GameTime elapsed)
        {
            canAfford = GameState.Score.Affordable(Upgrade.CurrentPrice);

            if (!Environment.IsPaused)
            {
                justBoughtDuration = justBoughtDuration.Tick();

                if (canAfford && Environment.Input.TryConsumeLeftMouseButtonClick(bounds))
                {
                    if (GameState.Score.TrySpend(Upgrade.CurrentPrice))
                    {
                        var levelUps =
                            Environment.Input.IsControlKeyDown()
                                ? Environment.Input.IsShiftKeyDown()
                                    ? SHIFT_LEVELS_COUNT
                                    : CTRL_LEVELS_COUNT
                                : 1;

                        Upgrade.LevelUp(levelUps);
                        Schedule(new SpawnObject(new HeartParticle(false, Environment.Input.Mouse.Position)));
                    }

                    justBoughtDuration = justBoughtDuration.Reset();
                }
            }
        }

        protected override void OnInitialized()
        {
            font = Environment.Resources.Fonts.Standard;
        }
    }
}
