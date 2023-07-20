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

    private static IServiceCollection UseConfig(this IServiceCollection serviceCollection, HostBuilderContext context)
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

        serviceCollection.AddSingleton<IGrpcHttpClient, GrpcHttpClient>();

        serviceCollection.AddSingleton<CookieContainer>();
        serviceCollection.ConfigureAll<HttpClientFactoryOptions>(options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                builder.PrimaryHandler = new HttpClientHandler()
                {
#if !DEBUG
                  Proxy = null,
                    UseProxy = false,
#endif
                    UseCookies = true,
                    CookieContainer = builder.Services.GetRequiredService<CookieContainer>()
                };
                builder.AdditionalHandlers.Add(builder.Services.GetRequiredService<HttpHeaderHandler>());
            });
        });

        return serviceCollection;
    }

    private static IServiceCollection UseApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IPassportApi, PassportApi>();
        serviceCollection.AddTransient<IAppApi, AppApi>();
        serviceCollection.AddTransient<IGrpcApi, GrpcApi>();
        return serviceCollection;
    }


    private static IServiceCollection UseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IJsonFileService, JsonFileService>();
        serviceCollection.AddSingleton<IUserSecretService, UserSecretService>();
        serviceCollection.AddSingleton<IApiErrorCodeHandlerService, ApiErrorCodeHandlerService>();
        serviceCollection.AddSingleton<IAccountService, AccountService>();

        serviceCollection.AddSingleton<ICookieService, CookieService>();

        return serviceCollection;
    }

    private static IServiceCollection UseHost(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<BilibiliClientCoreHostedService>();
        return serviceCollection;
    }

    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.UseUtils();
        services.UseConfig(context);
        services.UseMessenger();
        services.UsePlatformConfig();
        services.UseHttp();
        services.UseApi();
        services.UseServices();
        services.UseHost();
    }
}