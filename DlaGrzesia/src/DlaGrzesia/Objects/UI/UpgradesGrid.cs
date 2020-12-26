using Microsoft.Xna.Framework;

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

        public Rectangle GetIndexBounds(int index)
        {
            var positionInGrid = new Point(index % columns, index / columns);
            var position = location + positionInGrid * (size + margin);
            return new Rectangle(position, size);
        }
    }
}
