using System.IO;

namespace DlaGrzesia.Serialization
{
    public interface ISerializable
    {
        void Serialize(Stream stream, GameStateSerializer serializer);
        void Deserialize(Stream stream, GameStateSerializer serializer);
    }
}
