using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DlaGrzesia.Serialization
{
    public class GameStateRepository
    {
        private const string FILE_NAME = "save";

        private static string FilePath => $@"{AppDomain.CurrentDomain.BaseDirectory}\{FILE_NAME}";

        public bool FileExists => File.Exists(FilePath);

        public GameState Load(IEnumerable<IDeserializerFactory> deserializerFactories)
        {
            var factories = deserializerFactories.ToDictionary(x => x.Type);
            var objects = new List<ISerializable>();
            int totalScore;

            using var stream = new FileStream(FilePath, FileMode.Open);

            try
            {
                totalScore = stream.ReadInt();
                while (stream.TryReadVarchar(out var type))
                {
                    var @object = factories[type].CreateAndDeserialize(stream);
                    objects.Add(@object);
                }
            }
            finally
            {
                stream.Close();
            }

            return new GameState(objects, totalScore);
        }

        public void Save(GameState state)
        {
            using var stream = new FileStream(FilePath, FileMode.Create);

            try
            {
                stream.WriteInt(state.TotalScore);

                foreach (var @object in state.Objects)
                {
                    var type = @object.GetType().FullName;
                    stream.WriteVarchar(type);
                    @object.Serialize(stream);
                }
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
