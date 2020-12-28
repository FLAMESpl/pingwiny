using DlaGrzesia.Assets;
using DlaGrzesia.Objects.Particles;
using DlaGrzesia.Serialization;
using System.IO;

namespace DlaGrzesia.Objects.Actors
{
    public class SlidingPenguinDeserializationFactory : PenguinDeserializerFactory, IDeserializerFactory
    {
        private readonly Textures textures;

        public SlidingPenguinDeserializationFactory(
            Textures textures,
            Fonts fonts) : base(fonts)
        {
            this.textures = textures;
        }

        public string Type { get; } = typeof(SlidingPenguin).FullName;

        public ISerializable CreateAndDeserialize(Stream stream)
        {
            var penguin = new SlidingPenguin(DeserializeBase(stream, textures.PenguinSliding));
            penguin.Deserialize(stream);
            return penguin;
        }
    }

    public class SurfingPenguinDeserializationFactory : PenguinDeserializerFactory, IDeserializerFactory
    {
        private readonly Textures textures;

        public SurfingPenguinDeserializationFactory(
            Textures textures,
            Fonts fonts) : base(fonts)
        {
            this.textures = textures;
        }

        public string Type { get; } = typeof(SurfingPenguin).FullName;

        public ISerializable CreateAndDeserialize(Stream stream)
        {
            var penguin = new SurfingPenguin(DeserializeBase(stream, textures.PenguinWithBoard));
            penguin.Deserialize(stream);
            return penguin;
        }
    }

    public class WalkingPenguinDeserializationFactory : PenguinDeserializerFactory, IDeserializerFactory
    {
        private readonly Textures textures;

        public WalkingPenguinDeserializationFactory(
            Textures textures,
            Fonts fonts) : base(fonts)
        {
            this.textures = textures;
        }
        public string Type { get; } = typeof(WalkingPenguin).FullName;

        public ISerializable CreateAndDeserialize(Stream stream)
        {
            var penguin = new WalkingPenguin(DeserializeBase(stream, textures.PenguinWalking));
            penguin.Deserialize(stream);
            return penguin;
        }
    }

    public abstract class PenguinDeserializerFactory
    {
        private readonly Fonts fonts;

        public PenguinDeserializerFactory(Fonts fonts)
        {
            this.fonts = fonts;
        }

        protected PenguinBase DeserializeBase(Stream stream, Tileset tileset)
        {
            var @base = new PenguinBase(
                tileset,
                fonts.Font,
                default,
                default,
                default,
                default,
                default);

            @base.Deserialize(stream);
            return @base;
        }
    }

    public class PenguinGeneratorDeserializerFactory : IDeserializerFactory
    {
        private readonly Textures textures;
        private readonly Fonts fonts;

        public PenguinGeneratorDeserializerFactory(
            Textures textures, 
            Fonts fonts)
        {
            this.textures = textures;
            this.fonts = fonts;
        }

        public string Type { get; } = typeof(PenguinGenerator).FullName;

        public ISerializable CreateAndDeserialize(Stream stream)
        {
            var generator = new PenguinGenerator(textures, fonts);
            generator.Deserialize(stream);
            return generator;
        }
    }
}
