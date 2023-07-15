using BilibiliClient.Core.Api;
using BilibiliClient.Core.ApiHttpClient;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
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


    private static IServiceCollection UseHttp(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<HttpHeaderHandler>();
        serviceCollection.Replace(ServiceDescriptor
            .Singleton<IHttpMessageHandlerBuilderFilter, TraceIdLoggingMessageHandlerFilter>());

        serviceCollection.AddHttpClient<IPassportHttpClient, PassportHttpClient>();
        serviceCollection.AddTransient<IAppHttpClient, AppHttpClient>();

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
        serviceCollection.AddTransient<IAccountApi, AccountApi>();
        serviceCollection.AddTransient<IAppApi, AppApi>();
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
        services.UseHttp();
        services.UseApi();
        services.UseServices();
        services.UseHost();
    }
}