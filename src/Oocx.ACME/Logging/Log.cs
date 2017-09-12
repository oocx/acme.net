namespace Oocx.ACME.Common
{
    public class Log
    {
        public static LogLevel Level { get; set; }
        public static ILog Current { get; set; } = new ConsoleLogger();

        public static void Verbose(string message)
        {
            if (Level < LogLevel.Verbose) return;
            Current.Verbose(message);
        }

        public static void Info(string message)
        {
            if (Level < LogLevel.Info) return;
            Current.Info(message);
        }

        public static void Warning(string message)
        {
            if (Level < LogLevel.Warning) return;
            Current.Warning(message);
        }

        public static void Error(string message)
        {
            if (Level < LogLevel.Error) return;
            Current.Error(message);
        }
    }
}