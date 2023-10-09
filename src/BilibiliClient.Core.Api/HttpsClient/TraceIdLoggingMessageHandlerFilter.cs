using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.Api.HttpsClient;

internal class TraceIdLoggingMessageHandlerFilter(ILoggerFactory loggerFactory) : IHttpMessageHandlerBuilderFilter
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        if (next == null)
        {
            throw new ArgumentNullException(nameof(next));
        }

        return (builder) =>
        {
            // Run other configuration first, we want to decorate.
            next(builder);
            //var aa = new LoggingScopeHttpMessageHandler();
            //var aa = LoggingHttpMessageHandler;
            var outerLogger = _loggerFactory.CreateLogger(
                $"{(string.IsNullOrWhiteSpace(builder.Name) ? "UnNamedHttpClient" : builder.Name)}.{nameof(HttpLogHandler)}");

            RemoveOtherLogger(builder.AdditionalHandlers, typeof(LoggingScopeHttpMessageHandler));
            RemoveOtherLogger(builder.AdditionalHandlers, typeof(LoggingHttpMessageHandler));
            builder.AdditionalHandlers.Add(new HttpLogHandler(outerLogger));
        };
    }

    private void RemoveOtherLogger(IList<DelegatingHandler> additionalHandlers, Type type)
    {
        var item = additionalHandlers.FirstOrDefault(item => item.GetType() == type);
        if (item != null)
        {
            additionalHandlers.Remove(item);
        }
    }
}