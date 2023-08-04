using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.Api.HttpsClient;

internal class AppHttpClient : AbsHttpClient, IAppHttpClient
{
    public AppHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger<AppHttpClient> logger) : base(httpClient, jsonUtils,
        apiErrorHandlers, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.AppUrl);
    }
}