using System.Net;
using BilibiliClient.Core.Api;
using BilibiliClient.Core.ApiHttpClient;
using BilibiliClient.Core.Configs;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Configs;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using BilibiliClient.Core.Services;
using BilibiliClient.Core.Utils;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;

namespace BilibiliClient.Core.Extensions;

public static class BilibiliClientCoreExtensions
{
    private static IServiceCollection UseUtils(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJsonUtils, TextJsonUtils>();
        return serviceCollection;
    }

    private static IServiceCollection UseConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<UserSecretConfig>();
        return serviceCollection;
    }

    private static IServiceCollection UseMessenger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMessenger, WeakReferenceMessenger>();
        return serviceCollection;
    }

    private static IServiceCollection UsePlatformConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPlatformConfig, WebPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, AndroidPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, IosPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, LoginPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, TvPlatformConfig>();
        return serviceCollection;
    }


    private static IServiceCollection UseHttp(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<HttpHeaderHandler>();
        serviceCollection.Replace(ServiceDescriptor
            .Singleton<IHttpMessageHandlerBuilderFilter, TraceIdLoggingMessageHandlerFilter>());

        serviceCollection.AddHttpClient<IPassportHttpClient, PassportHttpClient>();
        serviceCollection.AddHttpClient<IAppHttpClient, AppHttpClient>();
        serviceCollection.AddHttpClient<IApiHttpClient, BilibiliClient.Core.ApiHttpClient.ApiHttpClient>();

        serviceCollection.AddSingleton<IGrpcHttpClient, GrpcHttpClient>();

        serviceCollection.AddSingleton<CookieContainer>();
        serviceCollection.AddScoped<HttpClientHandler>();
        serviceCollection.ConfigureAll<HttpClientFactoryOptions>(options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                var primaryHandler = builder.Services.GetRequiredService<HttpClientHandler>();
                var cookieContainer = builder.Services.GetRequiredService<CookieContainer>();
#if !DEBUG
                primaryHandler.Proxy = null;
                primaryHandler.UseProxy = false;
#endif
                if (!OperatingSystem.IsBrowser())
                {
                    primaryHandler.UseCookies = true;
                    primaryHandler.CookieContainer = cookieContainer;
                }

                builder.PrimaryHandler = primaryHandler;
                builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<HttpHeaderHandler>());
            });
        });

        return serviceCollection;
    }

    private static IServiceCollection UseApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IPassportApi, PassportApi>();
        serviceCollection.AddTransient<IAppApi, AppApi>();
        serviceCollection.AddTransient<IApiApi, ApiApi>();
        serviceCollection.AddTransient<IGrpcApi, GrpcApi>();
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
        serviceCollection.AddSingleton<IJsonFileService, JsonFileService>();
        serviceCollection.AddSingleton<IUserSecretService, UserSecretService>();
        serviceCollection.AddSingleton<IAccountService, AccountService>();
        serviceCollection.AddSingleton<IHistoryService, HistoryService>();
        serviceCollection.AddSingleton<IDynamicService, DynamicService>();

        serviceCollection.AddSingleton<ICookieService, CookieService>();

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