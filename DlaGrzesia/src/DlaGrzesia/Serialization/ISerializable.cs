using System.IO;

namespace DlaGrzesia.Serialization
{
    public interface ISerializable
    {
        void Serialize(Stream stream);
        void Deserialize(Stream stream);
    }
}
