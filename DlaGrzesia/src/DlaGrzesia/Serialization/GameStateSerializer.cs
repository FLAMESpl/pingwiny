using System;
using System.Collections.Generic;
using System.IO;

namespace DlaGrzesia.Serialization
{
    public class GameStateSerializer
    {
        public GameState Deserialize(Stream stream)
        {
            var state = ReadNext(stream);
            return (GameState)state;
        }

        public void Serialize(Stream stream, GameState state)
        {
            WriteNext(stream, state);
        }

        public ISerializable ReadNext(Stream stream)
        {
            if (TryReadNext(stream, out var result))
                return result;
            else
                throw new EndOfStreamException();
        }

        public bool TryReadNext(Stream stream, out ISerializable serializableGameState)
        {
            if (stream.TryReadVarchar(out var typeName))
            {
                var type = Type.GetType(typeName);
                serializableGameState = (ISerializable)Activator.CreateInstance(type, nonPublic: true);
                serializableGameState.Deserialize(stream, this);
                return true;
            }
            else
            {
                serializableGameState = null;
                return false;
            }
        }

        public void WriteNext(Stream stream, ISerializable serializableGameState)
        {
            stream.WriteType(serializableGameState.GetType());
            serializableGameState.Serialize(stream, this);
        }
    }
}
