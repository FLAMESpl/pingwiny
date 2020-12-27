using DlaGrzesia.Assets;
using DlaGrzesia.Mechanics;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Upgrades;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DlaGrzesia.Objects.UI
{
    public class UpgradeDisplay : IObject
    {
        private readonly Tileset tileset;
        private readonly SpriteFont font;
        private readonly Rectangle bounds;
        private Upgrade info;
        private bool canAfford = false;
        private Counter justBoughtDuration = Counter.NewElapsed(48);

        public UpgradeDisplay(Tileset tileset, SpriteFont font, Rectangle bounds, int index)
        {
            this.tileset = tileset;
            this.font = font;
            this.bounds = bounds;
            Index = index;
        }

        public bool Expired => false;
        public int Index { get; }

        private Point PriceLocation => new Point(bounds.Right - 10, bounds.Bottom - 40);
        private Point CountLocation => new Point(bounds.Center.X + 75, bounds.Center.Y);

        public void Draw(GameTime elapsed, SpriteBatch spriteBatch, DrawingModifiers modifiers)
        {
            var priceColor = canAfford ? Color.Red : Color.DarkGray;

            if (info.Count == 0)
            {
                spriteBatch.DrawCenteredString(font, info.Price.ToString(), bounds.Center, priceColor, 1f, LayerDepths.UI);
            }
            else
            {
                var tileIndex = modifiers.IsGamePaused ? 2 : justBoughtDuration.Elapsed ? 0 : 1;
                spriteBatch.Draw(tileset, bounds.Location, tileIndex, LayerDepths.UI);
                spriteBatch.DrawStringAlignedToLeft(font, info.Price.ToString(), PriceLocation, priceColor, 0.5f, LayerDepths.UI);
                spriteBatch.DrawCenteredString(font, info.Count.ToString(), CountLocation, Color.Black, 1, LayerDepths.UI);
            }
        }

        public void Update(GameTime elapsed, EnvironmentState environmentState)
        {
            info = environmentState.Upgrades[Index];
            canAfford = environmentState.Score.Affordable(info.Price);

            if (!environmentState.IsGamePaused)
            {
                justBoughtDuration = justBoughtDuration.Tick();

                if (canAfford && environmentState.Input.TryConsumeLeftMouseButtonClick(bounds))
                {
                    environmentState.Score.Spend(info.Price);
                    environmentState.Upgrades.Buy(Index);
                    environmentState.HeartsGenerator.Spawn(new HeartsParticle(false, environmentState.Input.Mouse.Position));
                    justBoughtDuration = justBoughtDuration.Reset();
                }
            }
        }
    }
}
