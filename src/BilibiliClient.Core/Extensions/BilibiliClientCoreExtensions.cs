using BilibiliClient.Core.Api.Contracts.Utils;
using BilibiliClient.Core.Api.Extensions;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Services;
using BilibiliClient.Core.Utils;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BilibiliClient.Core.Extensions;

public static class BilibiliClientCoreExtensions
{
    private static IServiceCollection UseUtils(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJsonUtils, TextJsonUtils>();
        return serviceCollection;
    }

    private static IServiceCollection UseMessenger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMessenger, WeakReferenceMessenger>();
        return serviceCollection;
    }

    private class LazilyResolved<T> : Lazy<T> where T : notnull
    {
        public LazilyResolved(IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<T>)
        {
        }
    }

    private static IServiceCollection UseLazyResolution(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient(typeof(Lazy<>), typeof(LazilyResolved<>));
        return serviceCollection;
    }

    private static IServiceCollection UseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICookieService, CookieService>();
        serviceCollection.AddSingleton<IJsonFileService, JsonFileService>();
        serviceCollection.AddSingleton<IUserSecretService, UserSecretService>();
        serviceCollection.AddSingleton<IAccountService, AccountService>();
        serviceCollection.AddSingleton<IHistoryService, HistoryService>();
        serviceCollection.AddSingleton<IDynamicService, DynamicService>();
        serviceCollection.AddSingleton<IPopularService, PopularService>();
        serviceCollection.AddSingleton<IRecommendService, RecommendService>();

        return serviceCollection;
    }

    private static void UseHost(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<BilibiliClientCoreHostedService>();
    }

    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.UseUtils()
            .UseConfig()
            .UseMessenger()
            .UsePlatformConfig()
            .UseHttp()
            .UseApi()
            .UseLazyResolution()
            .UseServices()
            .UseHost();
    }
}