using System.Net;
using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Configs;
using BilibiliClient.Core.Api.HttpsClient;
using BilibiliClient.Core.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace BilibiliClient.Core.Api.Extensions;

internal static class BilibiliClientCoreApiExtensions
{
    internal static IServiceCollection UseConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<UserSecretConfig>();
        return serviceCollection;
    }

    internal static IServiceCollection UsePlatformConfig(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPlatformConfig, WebPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, AndroidPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, IosPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, LoginPlatformConfig>();
        serviceCollection.AddSingleton<IPlatformConfig, TvPlatformConfig>();
        return serviceCollection;
    }


    internal static IServiceCollection UseHttp(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<HttpHeaderHandler>();
        serviceCollection.Replace(ServiceDescriptor
            .Singleton<IHttpMessageHandlerBuilderFilter, TraceIdLoggingMessageHandlerFilter>());

        serviceCollection.AddHttpClient<IPassportHttpClient, PassportHttpClient>();
        serviceCollection.AddHttpClient<IAppHttpClient, AppHttpClient>();
        serviceCollection.AddHttpClient<IApiHttpClient, ApiHttpClient>();

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

    internal static IServiceCollection UseApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IPassportApi, PassportApi>();
        serviceCollection.AddTransient<IAppApi, AppApi>();
        serviceCollection.AddTransient<IApiApi, ApiApi>();
        serviceCollection.AddTransient<IGrpcApi, GrpcApi>();
        return serviceCollection;
    }

}