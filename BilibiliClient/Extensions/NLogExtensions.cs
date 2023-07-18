using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avalonia;
using Avalonia.Logging;
using Avalonia.Utilities;
using NLog;
using NLog.Config;
using NLog.Targets;
using Logger = NLog.Logger;

namespace BilibiliClient.Extensions;


public static class NLogExtensions
{
    private static readonly Dictionary<LogEventLevel, LogLevel> LogLevelDic = new()
    {
        { LogEventLevel.Debug, LogLevel.Debug },
        { LogEventLevel.Error, LogLevel.Error },
        { LogEventLevel.Information, LogLevel.Info },
        { LogEventLevel.Verbose, LogLevel.Trace },
        { LogEventLevel.Fatal, LogLevel.Fatal },
        { LogEventLevel.Warning, LogLevel.Warn },
    };

    private static LogLevel GetLogLevel(this LogEventLevel logEventLevel)
    {
        LogLevelDic.TryGetValue(logEventLevel, out LogLevel? logLevel);
        return logLevel ?? LogLevel.Off;
    }

    private class AvaloniaNLoggerSink : ILogSink
    {
        private readonly LogEventLevel _level;
        private readonly ILogger _logger;

        private readonly IList<string>? _areas;

        public AvaloniaNLoggerSink(ILogger logger, IList<string>? areas = null)
        {
            _logger = logger;
            _level = logger.GetLogEventLevel();
            this._areas = areas is not { Count: > 0 } ? null : areas;
        }

        public bool IsEnabled(LogEventLevel level, string area)
        {
            if (level < this._level)
                return false;
            IList<string>? areas = this._areas;
            return areas == null || areas.Contains(area);
        }

        void ILogSink.Log(LogEventLevel level, string area, object? source, string messageTemplate)
        {
            if (!this.IsEnabled(level, area))
                return;
            _logger.Log(level.GetLogLevel(), Format<object, object, object>(area, messageTemplate, source));
        }

        void ILogSink.Log(LogEventLevel level, string area, object? source, string messageTemplate,
            params object?[] propertyValues)
        {
            if (!this.IsEnabled(level, area))
                return;
            _logger.Log(level.GetLogLevel(), Format(area, messageTemplate, source, propertyValues));
        }

        private static string Format<T0, T1, T2>(
            string area,
            string template,
            object? source,
            object?[]? values = null)
        {
            StringBuilder sb = new StringBuilder();
            CharacterReader characterReader = new CharacterReader(template.AsSpan());
            int num1 = 0;
            sb.Append('[');
            sb.Append(area);
            sb.Append("] ");
            while (!characterReader.End)
            {
                char ch = characterReader.Take();
                if (ch != '{')
                    sb.Append(ch);
                else if (characterReader.Peek != '{')
                {
                    sb.Append('\'');
                    sb.Append(values?[num1++]);
                    sb.Append('\'');
                    characterReader.TakeUntil('}');
                    int num2 = (int)characterReader.Take();
                }
                else
                {
                    sb.Append('{');
                    int num3 = (int)characterReader.Take();
                }
            }

            if (source != null)
            {
                sb.Append(" (");
                sb.Append(source.GetType().Name);
                sb.Append(" #");
                sb.Append(source.GetHashCode());
                sb.Append(')');
            }

            return sb.ToString();
        }

        private static string Format(string area, string template, object? source, object?[] v)
        {
            StringBuilder sb = new StringBuilder();
            CharacterReader characterReader = new CharacterReader(template.AsSpan());
            int num1 = 0;
            sb.Append('[');
            sb.Append(area);
            sb.Append(']');
            while (!characterReader.End)
            {
                char ch = characterReader.Take();
                if (ch != '{')
                    sb.Append(ch);
                else if (characterReader.Peek != '{')
                {
                    sb.Append('\'');
                    sb.Append(num1 < v.Length ? v[num1++] : (object?)null);
                    sb.Append('\'');
                    characterReader.TakeUntil('}');
                    int num2 = (int)characterReader.Take();
                }
                else
                {
                    sb.Append('{');
                    int num3 = (int)characterReader.Take();
                }
            }

            if (source != null)
            {
                sb.Append('(');
                sb.Append(source.GetType().Name);
                sb.Append(" #");
                sb.Append(source.GetHashCode());
                sb.Append(')');
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// configure avalonia logger to NLog
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="logger"></param>
    /// <param name="areas"></param>
    /// <returns></returns>
    public static AppBuilder LogToNLog(this AppBuilder builder,
        Logger logger,
        params string[] areas)
    {
        Avalonia.Logging.Logger.Sink = new AvaloniaNLoggerSink(logger, areas);
        return builder;
    }

    /// <summary>
    /// get LogEventLevel By NLog.Logger
    /// </summary>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static LogEventLevel GetLogEventLevel(this ILogger logger)
    {
        var logLevel = LogEventLevel.Error;
        if (logger.IsDebugEnabled)
        {
            logLevel = LogEventLevel.Debug;
        }
        else if (logger.IsTraceEnabled)
        {
            logLevel = LogEventLevel.Verbose;
        }
        else if (logger.IsErrorEnabled)
        {
            logLevel = LogEventLevel.Error;
        }
        else if (logger.IsWarnEnabled)
        {
            logLevel = LogEventLevel.Warning;
        }
        else if (logger.IsInfoEnabled)
        {
            logLevel = LogEventLevel.Information;
        }
        else if (logger.IsFatalEnabled)
        {
            logLevel = LogEventLevel.Fatal;
        }

        return logLevel;
    }

    /// <summary>
    /// Configure Nlog
    /// </summary>
    /// <param name="appName"></param>
    public static void ConfigNLog(string appName)
    {
        // Step 1. Create configuration object 
        var config = new LoggingConfiguration();
        // Step 2. Create targets and add them to the configuration 
        var consoleTarget = new ColoredConsoleTarget();
        config.AddTarget("console", consoleTarget);

        var fileTarget = new FileTarget();
        config.AddTarget("file", fileTarget);

        // Step 3. Set target properties 
        consoleTarget.Layout = @"${date:format=HH\:MM\:ss} ${logger} ${message}";
        fileTarget.FileName = "${basedir}/logs/" + appName + "_${shortdate}.log";
        fileTarget.Layout = @"${date:format=HH\:mm\:ss} ${uppercase:${level}} ${message}";
        fileTarget.MaxArchiveFiles = 10;
        fileTarget.ArchiveAboveSize = 10485760;
        // ReSharper disable once RedundantAssignment
        var minLevel = LogLevel.Error;
#if DEBUG
        minLevel = LogLevel.Debug;
#endif
        if (ShouldTrace(appName))
        {
            minLevel = LogLevel.Trace;
        }

        // Step 4. Define rules
        var rule1 = new LoggingRule("*", minLevel, consoleTarget);
        config.LoggingRules.Add(rule1);

        var rule2 = new LoggingRule("*", minLevel, fileTarget);
        config.LoggingRules.Add(rule2);

        LogManager.Configuration = config;
    }

    /// <summary>
    /// should Use Trace level
    /// </summary>
    /// <param name="appName"></param>
    /// <returns></returns>
    private static bool ShouldTrace(string appName)
    {
        try
        {
            var traceFlagPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"{appName}Debug");
            return File.Exists(traceFlagPath);
        }
        catch
        {
            // ignored
        }

        return false;
    }
}