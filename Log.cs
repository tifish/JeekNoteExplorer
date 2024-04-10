global using static JeekNoteExplorer.LogSingletonContainer;
using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace JeekNoteExplorer;

public static class LogSingletonContainer
{
    private static Logger? _log;

    public static Logger Log
    {
        get
        {
            if (_log != null)
                return _log;

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(
                    Path.Combine(AppSettings.ExeDirectory, @"Logs\JeekNoteExplorer_.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileTimeLimit: TimeSpan.FromDays(7),
                    restrictedToMinimumLevel: Debugger.IsAttached ? LogEventLevel.Verbose : LogEventLevel.Information
                );

            if (Debugger.IsAttached)
                loggerConfig.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Verbose);

            _log = loggerConfig.CreateLogger();

            return _log;
        }
    }
}
