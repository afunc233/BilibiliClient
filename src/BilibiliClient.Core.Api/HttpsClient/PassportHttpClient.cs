using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.Api.HttpsClient;

internal class PassportHttpClient : AbsHttpClient, IPassportHttpClient
{
    public PassportHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger<PassportHttpClient> logger) : base(httpClient, jsonUtils,
        apiErrorHandlers, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.PassportUrl);
    }

    public async ValueTask<HttpResponseMessage> Send4ResponseAsync(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
    }
}