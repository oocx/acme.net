using System;

namespace Oocx.Acme.Services
{
    public class ConsoleLogger : ILog
    {
        private void WriteLine(ConsoleColor color, string message)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = previous;
        }

        public void Verbose(string message)
        {
            WriteLine(ConsoleColor.DarkGray, message);
        }

        public void Info(string message)
        {
            WriteLine(ConsoleColor.Gray, message);
        }

        public void Warning(string message)
        {
            WriteLine(ConsoleColor.Yellow, message);
        }

        public void Error(string message)
        {
            WriteLine(ConsoleColor.Red, message);
        }
    }
}