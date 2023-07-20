using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using BilibiliClient.Core.Models.Https;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

/// <summary>
/// HttpClient 封装， 返回数据的主体 为 ApiResponse
/// </summary>
public abstract class AbsHttpClient : AbsHttpClient<ApiResponse>
{
    protected AbsHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger logger) : base(httpClient, jsonUtils,
        apiErrorCodeHandlerService, logger)
    {
    }

    protected override long GetErrorCode(ApiResponse api)
    {
        return api.Code;
    }

    protected override bool IsErrorCode(ApiResponse api)
    {
        return api.Code != 0;
    }

    protected override string? GetErrorMessage(ApiResponse api)
    {
        return api.Message;
    }

    protected override object? GetContent(ApiResponse api)
    {
        return api.Data;
    }
}