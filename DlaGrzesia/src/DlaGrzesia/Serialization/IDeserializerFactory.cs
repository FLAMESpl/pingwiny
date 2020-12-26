using System.IO;

namespace DlaGrzesia.Serialization
{
    public interface IDeserializerFactory
    {
        string Type { get; }
        ISerializable CreateAndDeserialize(Stream stream);
    }
}
