using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using BilibiliClient.Extensions;
using NLog;

namespace BilibiliClient.Desktop;

// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once ArrangeTypeModifiers
class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        const string appName = $"{nameof(BilibiliClient)}.{nameof(Desktop)}";
        NLogExtensions.ConfigNLog(appName);
        var logger = LogManager.GetCurrentClassLogger();
        try
        {
            TaskScheduler.UnobservedTaskException += (_, e) =>
            {
                logger.Error(e.Exception, $"{nameof(TaskScheduler.UnobservedTaskException)}！");
                e.SetObserved();
            };

            var builder = BuildAvaloniaApp();
            if (logger.IsTraceEnabled)
            {
                builder.LogToNLog(logger);
            }

            builder.StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "程序发生未捕获异常！");
            logger.Error(ex);
        }
        finally
        {
            logger.Error($"程序退出！");
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
#if !ARM64
            .With(new Win32PlatformOptions()
            {
                RenderingMode = new Win32RenderingMode[] { Win32RenderingMode.AngleEgl }
            })
#else
            .With(new Win32PlatformOptions()
            {
                CompositionMode = new List<Win32CompositionMode>() { Win32CompositionMode.WinUIComposition },
                WinUICompositionBackdropCornerRadius = 20,
            })
#endif
            .With(new FontManagerOptions()
            {
                // DefaultFamilyName = $"F:\\OpenSourceProject\\pumpkin-windows\\PumpkinDesktop\\Assets\\Fonts/msyh.ttf#Microsoft YaHei",
                // DefaultFamilyName = $"avares://{nameof(PumpkinDesktop)}/Assets/Fonts/msyh.ttf#Microsoft YaHei",
                // 
                DefaultFamilyName =
                    $"avares://{nameof(BilibiliClient)}/Assets/Fonts/LXGWWenKaiLite-Regular.ttf#LXGW WenKai Lite",
            })
            .UseReactiveUI();
}