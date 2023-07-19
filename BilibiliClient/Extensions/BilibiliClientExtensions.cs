using System;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Services;
using BilibiliClient.Services.Handler;
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

        return serviceCollection;
    }

    public static IServiceCollection UseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IWindowManagerService, WindowManagerService>();

        serviceCollection.AddSingleton<IStartStopHandler, UserSecretStartStopHandler>();

        return serviceCollection;
    }
}