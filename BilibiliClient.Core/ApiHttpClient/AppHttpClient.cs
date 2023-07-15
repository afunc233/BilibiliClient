using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using BilibiliClient.Core.Models.Https;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class AppHttpClient : ApiAbsHttpClient, IAppHttpClient
{
    public AppHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger<AppHttpClient> logger) : base(httpClient, jsonUtils,
        apiErrorCodeHandlerService, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.AppUrl);
    }
}