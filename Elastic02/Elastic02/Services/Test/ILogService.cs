namespace Elastic02.Services.Test
{
    public enum LogLevel
    {
        Error,
        Info,
        Debug,
        Warn,
        Fatal
    }

    public interface ILogService
    {
        bool WriteLog(string message, LogLevel level);
    }
}
