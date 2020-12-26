using DlaGrzesia.Assets;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DlaGrzesia
{
    public class Textures : IDisposable
    {
        [Resource]
        public Texture2D Heart { get; private set; }

        [Resource]
        public Texture2D Hearts { get; private set; }

        [Resource]
        public Texture2D Stage { get; private set; }

        [Tileset(64, 64)]
        public Tileset PenguinSliding { get; private set; }

        [Tileset(64, 64)]
        public Tileset PenguinWalking { get; private set; }

        [Resource]
        public Tileset PenguinWithBoard { get; private set; }

        [Resource]
        public Texture2D UIBackground { get; private set; }

        [Tileset(150, 150)]
        public Tileset Grzesiek { get; private set; }

        [Tileset(150, 150, 16)]
        public Tileset DOG { get; private set; }

        public void Dispose()
        {
            Heart?.Dispose();
            Hearts?.Dispose();
            Stage?.Dispose();
            PenguinSliding?.Dispose();
            PenguinWalking?.Dispose();
            PenguinWithBoard?.Dispose();
            UIBackground?.Dispose();
            Grzesiek?.Dispose();
            DOG?.Dispose();
        }
    }
}
