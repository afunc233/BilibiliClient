using System;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Services;
using BilibiliClient.Services.Handler;
using BilibiliClient.ViewModels;
using BilibiliClient.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BilibiliClient.Extensions;

public static class BilibiliClientExtensions
{
    public static IServiceCollection UseHost(this IServiceCollection serviceCollection)
    {
        var isDesktop = OperatingSystem.IsWindows() || OperatingSystem.IsLinux() || OperatingSystem.IsMacOS() ||
                        OperatingSystem.IsMacCatalyst();
        if (isDesktop)
        {
            serviceCollection.AddHostedService<AppTrayIconHostService>();
        }
        serviceCollection.AddHostedService<BilibiliHostedService>();
        return serviceCollection;
    }

    public static IServiceCollection UseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IWindowManagerService, WindowManagerService>();

        serviceCollection.AddSingleton<IStartStopHandler, UserSecretStartStopHandler>();

        return serviceCollection;
    }

    public static IServiceCollection UseViewModel(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddTransient<MainWindow>();

        serviceCollection.AddSingleton<IPageViewModel, RecommendPageViewModel>();
        serviceCollection.AddSingleton<IPageViewModel, PopularPageViewModel>();
        serviceCollection.AddTransient<IPageViewModel, HistoryPageViewModel>();
        serviceCollection.AddTransient<IPageViewModel, DynamicPageViewModel>();

        serviceCollection.AddSingleton<IPageViewModel, SettingPageViewModel>();

        serviceCollection.AddSingleton<HeaderViewModel>();
        serviceCollection.AddSingleton<LoginViewModel>();

        serviceCollection.AddSingleton<PlayerViewModel>();

        return serviceCollection;
    }

    public static IServiceCollection UseView(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<MainView>();
        serviceCollection.AddTransient<RecommendPageView>();
        serviceCollection.AddTransient<PopularPageView>();
        serviceCollection.AddTransient<HistoryPageView>();
        serviceCollection.AddTransient<DynamicPageView>();
        serviceCollection.AddTransient<SettingPageView>();
        serviceCollection.AddTransient<HeaderView>();
        serviceCollection.AddTransient<LoginView>();
        

        return serviceCollection;
    }
}