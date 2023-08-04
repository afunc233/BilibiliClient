using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Utils;
using BilibiliClient.Core.Models.Https;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.Api.HttpsClient;

/// <summary>
/// HttpClient 封装， 返回数据的主体 为 ApiResponse
/// </summary>
internal abstract class AbsHttpClient : AbsHttpClient<ApiResponse>
{
    protected AbsHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        IEnumerable<IApiErrorHandler> apiErrorHandlers, ILogger logger) : base(httpClient, jsonUtils,
        apiErrorHandlers, logger)
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