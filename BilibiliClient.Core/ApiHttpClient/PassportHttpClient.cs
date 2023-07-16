using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class PassportHttpClient : AbsHttpClient, IPassportHttpClient
{
    public PassportHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger<PassportHttpClient> logger) : base(
        httpClient, jsonUtils, apiErrorCodeHandlerService, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.PassportUrl);
    }
}