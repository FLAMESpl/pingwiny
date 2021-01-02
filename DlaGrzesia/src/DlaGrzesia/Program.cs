using System;
using System.IO;

namespace DlaGrzesia
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Game();

            try
            {
                game.Run();
            }
            catch (Exception exception)
            {
                LogToFile(exception);
                throw;
            }
        }

        static void LogToFile(Exception exception)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-f");
            var directoryPath = $@"{AppDomain.CurrentDomain.BaseDirectory}\logs";
            var filePath = $@"{directoryPath}\Crash_{timestamp}.log";

            Directory.CreateDirectory(directoryPath);
            File.WriteAllText(filePath, exception.ToString());
        }
    }
}
