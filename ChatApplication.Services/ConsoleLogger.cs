using ChatApplication.Domain;
using System;

namespace ChatApplication.Services
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine("LOG: " + message);
        }
    }
}
