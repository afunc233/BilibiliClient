using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;

[assembly: SupportedOSPlatform("browser")]

namespace BilibiliClient.Browser;
internal class Program
{
    private static async Task Main() => await BuildAvaloniaApp()
            .WithInterFont()
            .UseReactiveUI()
            .StartBrowserAppAsync("out");

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}