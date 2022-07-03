using Serilog.Core;
using Serilog.Events;
using System.Runtime.InteropServices;

namespace Infrastructure.Logging
{
    public static class LoggingExtension
    {
        public static Logger ConfigureLogger()
        {
            var separator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/";


            var logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: $"Logs{separator}api-log-.log",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                    ).CreateLogger();
            return logger;
        }
    }
}
