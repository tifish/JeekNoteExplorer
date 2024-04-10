global using static JeekNoteExplorer.LogSingletonContainer;
using System.Diagnostics;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace JeekNoteExplorer;

public static class LogSingletonContainer
{
    public static Logger Log { get; } = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.File(
            Path.Combine(AppSettings.ExeDirectory, @"Logs\JeekNoteExplorer_.log"),
            rollingInterval: RollingInterval.Day,
            retainedFileTimeLimit: TimeSpan.FromDays(7),
            restrictedToMinimumLevel: Debugger.IsAttached ? LogEventLevel.Debug : LogEventLevel.Information
        )
        .CreateLogger();
}
