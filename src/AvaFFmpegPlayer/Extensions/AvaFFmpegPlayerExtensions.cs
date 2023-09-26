using Avalonia;
using Avalonia.Logging;
using Avalonia.Markup.Xaml.Styling;
using Microsoft.Extensions.DependencyInjection;

namespace AvaFFmpegPlayer.Extensions;

public static class AvaFFmpegPlayerExtensions
{
    public static void UseFFmpeg(this IServiceCollection serviceCollection)
    {
        // serviceCollection.AddTransient<IVideoPlayer, FFmpegVideoPlayer>();
    }

    internal static bool IsInitialize { get; private set; }
    private static ParametrizedLogger? _parametrizedLogger = null;

    private static unsafe void InitializeFFmpeg(int logLevel = ffmpeg.AV_LOG_VERBOSE)
    {
        if (IsInitialize)
        {
            return;
        }

        var logEventLevel = logLevel switch
        {
            ffmpeg.AV_LOG_VERBOSE => LogEventLevel.Verbose,
            ffmpeg.AV_LOG_INFO => LogEventLevel.Information,
            ffmpeg.AV_LOG_DEBUG => LogEventLevel.Debug,
            ffmpeg.AV_LOG_FATAL => LogEventLevel.Fatal,
            ffmpeg.AV_LOG_ERROR => LogEventLevel.Error,
            ffmpeg.AV_LOG_WARNING => LogEventLevel.Warning,
            _ => LogEventLevel.Debug
        };
        _parametrizedLogger = Logger.TryGet(logEventLevel, $"{nameof(ffmpeg)}");
        try
        {
            var ffmpegPath = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
#if x86
                ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg", "x86");
#elif x64
                ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg", "x64");
#elif ARM64
                ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg", "Arm64");
#elif AnyCpu
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libFFmpeg", "x86");
                        break;
                    case Architecture.X64:
                        ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libFFmpeg", "x64");
                        break;
                    case Architecture.Arm64:
                        ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libFFmpeg", "Arm64");
                        break;
                    default:
                        throw new ApplicationException("un support Architecture");
                }
#endif
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ffmpegPath = "/usr/lib/";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ffmpegPath = "/usr/lib/";
            }

            ffmpeg.RootPath = ffmpegPath;
            ffmpeg.avformat_network_init();
            ffmpeg.av_log_set_level(logLevel);
            
            // ReSharper disable once ConvertToLocalFunction
            av_log_set_callback_callback logCallback = (p0, level, format, vl) =>
            {
                var logLevelNow = ffmpeg.av_log_get_level();
                if (level > logLevelNow) return;
                const int lineSize = 1024;
                var lineBuffer = stackalloc byte[lineSize];
                var printPrefix = 1;
                ffmpeg.av_log_format_line(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
                var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);
                if (!string.IsNullOrWhiteSpace(line))
                {
                    _parametrizedLogger?.Log(null, line);
                }
            };

            ffmpeg.av_log_set_callback(logCallback);

            try
            {
                ffmpeg.avdevice_register_all();
            }
            catch (Exception e)
            {
                _parametrizedLogger?.Log(null, $"avdevice_register_all error {e.Message}----->{e.StackTrace}");
            }

            IsInitialize = true;
        }
        catch (Exception ex)
        {
            _parametrizedLogger?.Log(null, $"{ex.Message}----->{ex.StackTrace}");
            IsInitialize = false;
        }
    }

    public static AppBuilder UseFFmpegView(this AppBuilder builder)
    {
        builder.AfterSetup((_) =>
        {
            InitXamlStyle(builder);
            InitBass(builder);
            InitializeFFmpeg();
        });
        return builder;
    }

    /// <summary>
    /// 初始化Bass 
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TAppBuilder"></typeparam>
    private static void InitBass<TAppBuilder>(TAppBuilder builder)
    {
        BassCore.Initialize();
    }

    #region 加载样式

    /// <summary>
    /// 加载样式
    /// </summary>
    /// <param name="builder"></param>
    private static void InitXamlStyle(object builder)
    {
        try
        {
            StyleInclude styleInclude = new StyleInclude(new Uri($"avares://{nameof(AvaFFmpegPlayer)}"))
            {
                Source = new Uri(
                    $"avares://{nameof(AvaFFmpegPlayer)}/{nameof(AvaFFmpegPlayer.Controls)}/VideoView.axaml")
            };
            Application.Current?.Styles.Add(styleInclude);
        }
        catch (Exception ex)
        {
            Logger.TryGet(LogEventLevel.Error, LogArea.Control)?.Log(builder, ex.Message);
        }
    }

    #endregion
}