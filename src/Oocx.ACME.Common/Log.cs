namespace Oocx.ACME.Common
{
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
}