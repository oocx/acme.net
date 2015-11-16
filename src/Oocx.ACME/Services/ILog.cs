using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oocx.ACME.Services
{
    public interface ILog
    {
        void Verbose(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warning(string message, params object[] args);

        void Error(string message, params object[] args);
    }

    public enum LogLevel
    {
        Disabled,
        Error,
        Warning,        
        Info,
        Verbose
    }

    public class Log
    {
        public static LogLevel Level { get; set; }
        public static ILog Current { get; set; } = new ConsoleLogger();

        public static void Verbose(string message, params object[] args)
        {
            if (Level < LogLevel.Verbose) return;
            Current.Verbose(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            if (Level < LogLevel.Info) return;
            Current.Info(message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            if (Level < LogLevel.Warning) return;
            Current.Warning(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            if (Level < LogLevel.Error) return;
            Current.Error(message, args);
        }
    }

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
