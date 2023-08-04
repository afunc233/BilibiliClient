using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.Api.HttpsClient;

internal class ApiHttpClient : AbsHttpClient, IApiHttpClient
{
    public ApiHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger<ApiHttpClient> logger) : base(httpClient, jsonUtils,
        apiErrorHandlers, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.ApiUrlUrl);
    }
}