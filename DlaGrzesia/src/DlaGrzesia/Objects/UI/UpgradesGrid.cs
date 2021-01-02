using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace DlaGrzesia.Objects.UI
{
    public class UpgradesGrid
    {
        private readonly Point location;
        private readonly Point size;
        private readonly int columns;
        private readonly Point margin;

        public UpgradesGrid(Point location, Point size, int columns, Point margin)
        {
            this.location = location;
            this.size = size;
            this.columns = columns;
            this.margin = margin;
        }

        public IEnumerable<UpgradeDisplay> CreateDisplays(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new UpgradeDisplay(GetIndexBounds(i), i);
            }
        }

        private Rectangle GetIndexBounds(int index)
        {
            var positionInGrid = new Point(index % columns, index / columns);
            var position = location + positionInGrid * (size + margin);
            return new Rectangle(position, size);
        }
    }
}
