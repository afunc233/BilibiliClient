using BilibiliClient.Core.Api;
using BilibiliClient.Core.ApiHttpClient;
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

        // TODO IAuthenticationProvider
        serviceCollection.AddSingleton<IAuthenticationProvider, AuthenticationProvider>();

        serviceCollection.AddHttpClient<IPassportHttpClient, PassportHttpClient>();
        serviceCollection.AddHttpClient<IAppHttpClient, AppHttpClient>();

        serviceCollection.AddSingleton<IGrpcHttpClient, GrpcHttpClient>();

        serviceCollection.ConfigureAll<HttpClientFactoryOptions>(options =>
        {
            options.HttpMessageHandlerBuilderActions.Add(builder =>
            {
                builder.PrimaryHandler = new HttpClientHandler()
                {
                    Proxy = null,
                    UseProxy = false
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
        serviceCollection.AddSingleton<IApiErrorCodeHandlerService, ApiErrorCodeHandlerService>();
        return serviceCollection;
    }

    private static IServiceCollection UseHost(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.UseUtils();
        services.UseMessenger();
        services.UsePlatformConfig();
        services.UseHttp();
        services.UseApi();
        services.UseServices();
        services.UseHost();
    }
}