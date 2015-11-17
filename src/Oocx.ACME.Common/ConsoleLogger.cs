using System;

namespace Oocx.ACME.Common
{
    public class ConsoleLogger : ILog
    {
        private void WriteLine(ConsoleColor color, string message, params object[] args)
        {
            var previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(string.Format(message, args));
            Console.ForegroundColor = previous;
        }

        public void Verbose(string message, params object[] args)
        {
            WriteLine(ConsoleColor.DarkGray, message, args);
        }

        public void Info(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Gray, message, args);
        }

        public void Warning(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Yellow, message, args);
        }

        public void Error(string message, params object[] args)
        {
            WriteLine(ConsoleColor.Red, message, args);
        }
    }
}