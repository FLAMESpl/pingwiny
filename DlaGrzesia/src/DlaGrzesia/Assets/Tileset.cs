using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DlaGrzesia.Assets
{
    public struct Tile
    {
        public readonly Texture2D Texture;
        public readonly Rectangle Source;

        public Tile(Texture2D texture, Rectangle source)
        {
            Texture = texture;
            Source = source;
        }
    }

    public class Tileset : IDisposable
    {
        class TilesetPart : IDisposable
        {
            public TilesetPart(int firstIndex, int lastIndex, Texture2D texture, Point gridSize)
            {
                FirstIndex = firstIndex;
                LastIndex = lastIndex;
                Texture = texture ?? throw new ArgumentNullException(nameof(texture));
                GridSize = gridSize;
            }

            public int FirstIndex { get; }
            public int LastIndex { get; }
            public Texture2D Texture { get; private set; }
            public Point GridSize { get; }

            public Tile SelectTile(int index, Point tileSize)
            {
                var indexInThisPart = index - FirstIndex;
                var positionInGrid = new Point(indexInThisPart % GridSize.X, indexInThisPart / GridSize.X);
                var source = new Rectangle(positionInGrid * tileSize, tileSize);
                return new Tile(Texture, source);
            }

            public void Dispose()
            {
                Texture.Dispose();
                Texture = null;
            }
        }

        private IReadOnlyList<TilesetPart> parts;

        public Tileset(IReadOnlyList<Texture2D> textures, Point tileSize)
        {
            TileSize = tileSize;

            var parts = new List<TilesetPart>(textures.Count);
            var tilesCountSoFar = 0;

            foreach (var (texture, index) in textures.Select((x, n) => (x, n)))
            {
                var textureGridSize = texture.Bounds.Size / tileSize;
                var firstIndex = tilesCountSoFar;
                tilesCountSoFar += textureGridSize.X * textureGridSize.Y;
                parts.Add(new TilesetPart(firstIndex, tilesCountSoFar - 1, texture, textureGridSize));
            }

            TilesCount = tilesCountSoFar;
            this.parts = parts;
        }

        public Point TileSize { get; }
        public int TilesCount { get; }

        public void Dispose()
        {
            foreach (var part in parts)
                part.Dispose();

            parts = null;
        }

        public Tile GetTile(int index)
        {
            return parts.First(x => x.LastIndex >= index).SelectTile(index, TileSize);
        }
    }
}
