using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class AppHttpClient : AbsHttpClient, IAppHttpClient
{
    public AppHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger<AppHttpClient> logger) : base(httpClient, jsonUtils,
        apiErrorHandlers, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.AppUrl);
    }
}