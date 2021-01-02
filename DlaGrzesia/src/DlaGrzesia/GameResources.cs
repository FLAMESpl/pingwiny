using System;

namespace DlaGrzesia
{
    public class GameResources : IDisposable
    {
        public GameResources(Textures textures, Fonts fonts)
        {
            Textures = textures;
            Fonts = fonts;
        }

        public Textures Textures { get; }
        public Fonts Fonts { get; }

        public void Dispose()
        {
            Textures.Dispose();
        }
    }
}
