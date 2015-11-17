namespace Oocx.ACME.Common
{
    public interface ILog
    {
        void Verbose(string message, params object[] args);

        void Info(string message, params object[] args);

        void Warning(string message, params object[] args);

        void Error(string message, params object[] args);
    }
}
