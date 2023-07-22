using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BilibiliClient.Core.Extensions;
using BilibiliClient.Extensions;
using BilibiliClient.ViewModels;
using BilibiliClient.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Hosting;

namespace BilibiliClient;

public class App : Application
{
    // ReSharper disable once InconsistentNaming
    private static readonly NLog.Logger? _logger;

    private IHost? _host;

    static App()
    {
        _logger = NLog.LogManager.GetCurrentClassLogger();
    }

    public override void Initialize()
    {
        _logger?.Debug($"{nameof(Initialize)}");
        if (OperatingSystem.IsWindows())
        {
            ThreadPool.GetMinThreads(out var workers, out var ports);
            ThreadPool.SetMinThreads(workers + 6, ports + 6);

            var process = Process.GetCurrentProcess();
            process.PriorityClass = ProcessPriorityClass.RealTime;
        }

        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();
        LibVLCSharp.Shared.Core.Initialize();
        var appLocation = Directory.GetCurrentDirectory();
        _host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configHost =>
            {
                configHost.SetBasePath(appLocation);
                configHost.AddJsonFile("hostsettings.json", optional: true);
                configHost.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // 日志配置 热更新
            })
            .ConfigureServices(ConfigureServices)
            .ConfigureServices(BilibiliClientCoreExtensions.ConfigureServices)
            .ConfigureServices((_, services) =>
            {
                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    services.AddSingleton(desktop);
                }
            })
            .UseNLog()
            .Build();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.UseHost();
        services.UseServices();
        services.UseViewModel();
        services.UseView();
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (_host == null)
        {
            return;
        }

        _logger?.Debug($"{nameof(_host.StartAsync)}");

        await _host.StartAsync();

        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:

                #region ShutdownRequested

                desktop.ShutdownRequested += async (_, _) =>
                {
                    try
                    {
                        await _host.StopAsync();
                        _host.Dispose();
                        _host = null;
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                    // 停 500 ms
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                };

                #endregion ShutdownRequested

                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnExplicitShutdown;

                var mainWindow = GetAppRequiredServiceInner<MainWindow>();
                mainWindow!.DataContext = GetAppRequiredServiceInner<MainViewModel>();
                desktop.MainWindow = mainWindow;
                break;
            case ISingleViewApplicationLifetime singleViewPlatform:
                // Android / iOS
                var mainView = GetAppRequiredServiceInner<MainView>();
                mainView!.DataContext = GetAppRequiredServiceInner<MainViewModel>();
                singleViewPlatform.MainView = mainView;
                break;
        }

        base.OnFrameworkInitializationCompleted();
    }


    public T? GetAppRequiredServiceInner<T>() where T : class
    {
        return _host?.Services.GetRequiredService<T>();
    }

    public T? GetAppServiceInner<T>(Type type) where T : class
    {
        return _host?.Services.GetService(type) as T;
    }
}

public static class AppEx
{
    public static T GetAppRequiredService<T>(this object _) where T : class
    {
        return (Application.Current as App)?.GetAppRequiredServiceInner<T>()!;
    }

    public static T? GetAppRequiredService<T>(this object _, Type type) where T : class
    {
        return (Application.Current as App)?.GetAppServiceInner<T>(type);
    }
}