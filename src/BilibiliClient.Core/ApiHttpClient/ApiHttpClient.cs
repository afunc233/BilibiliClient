using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class ApiHttpClient : AbsHttpClient, IApiHttpClient
{
    public ApiHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger<ApiHttpClient> logger) : base(httpClient, jsonUtils,
        apiErrorHandlers, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.ApiUrlUrl);
    }
}