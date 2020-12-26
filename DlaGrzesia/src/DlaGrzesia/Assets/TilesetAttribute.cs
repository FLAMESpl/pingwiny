using Microsoft.Xna.Framework;

namespace DlaGrzesia.Assets
{
    public class TilesetAttribute : ResourceAttribute
    {
        public TilesetAttribute(int tileWidth, int tileHeight)
        {
            TileSize = new Point(tileWidth, tileHeight);
            FilesCount = 1;
            IsMultipart = false;
        }

        public TilesetAttribute(int tileWidth, int tileHeight, int filesCount)
        {
            TileSize = new Point(tileWidth, tileHeight);
            FilesCount = filesCount;
            IsMultipart = true;
        }

        public Point TileSize { get; }
        public int FilesCount { get; }
        public bool IsMultipart { get; }
    }
}
