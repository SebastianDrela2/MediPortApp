using Serilog;

namespace MediPortSOAPI.Logging
{
    internal class SeriloggerFactory
    {
        private static readonly string _logPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\MediPortSOAPI\logs.txt";
        private static ILogger _logger;

        public static ILogger GetLogger()
        {
            if (_logger is not null)
            {
                return _logger;
            }

            var logger = new LoggerConfiguration().WriteTo.File(_logPath , rollingInterval: RollingInterval.Day).CreateLogger();

            return logger;
        }

    }
}
