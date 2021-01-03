using System;
using System.IO;

namespace DlaGrzesia.Serialization
{
    public class GameStateRepository
    {
        private const string FILE_NAME = "save";
        private GameStateSerializer serializer = new GameStateSerializer();

        private static string FilePath => $@"{AppDomain.CurrentDomain.BaseDirectory}\{FILE_NAME}";
        public bool FileExists => File.Exists(FilePath);

        public GameState Load()
        {
            using var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            try
            {
                return serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }
        }

        public void Save(GameState state)
        {
            using var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);

            try
            {
                serializer.Serialize(stream, state);
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
