using ChatApplication.Domain;
using System.IO;

namespace ChatApplication.Services
{
    public class FileLogger : ILogger
    {
        private readonly string _fileName = "logs.txt";

        public void Log(string message)
        {
            if (File.Exists(_fileName))
            {
                using var streamWriter = File.AppendText(_fileName);

                streamWriter.WriteLine("LOG: " + message);
            }
            else
            {
               using var streamWriter = File.CreateText(_fileName);

               streamWriter.WriteLine("LOG: " + message);
            }

        }
    }
}
