using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class PassportHttpClient : AbsHttpClient, IPassportHttpClient
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