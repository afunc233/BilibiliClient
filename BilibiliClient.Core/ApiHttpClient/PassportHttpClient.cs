using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Contracts.Utils;
using BilibiliClient.Core.Models.Https;
using Microsoft.Extensions.Logging;

namespace BilibiliClient.Core.ApiHttpClient;

public class PassportHttpClient : AbsHttpClient<ResponseModel>, IPassportHttpClient
{
    public PassportHttpClient(HttpClient httpClient, IJsonUtils jsonUtils,
        // ReSharper disable once ContextualLoggerProblem
        IApiErrorCodeHandlerService apiErrorCodeHandlerService, ILogger<AbsHttpClient<ResponseModel>> logger) : base(
        httpClient, jsonUtils, apiErrorCodeHandlerService, logger)
    {
        httpClient.BaseAddress = new Uri(ApiConstants.PassportUrl);
    }

    protected override long GetErrorCode(ResponseModel apiModel)
    {
        return apiModel.Code;
    }

    protected override bool IsErrorCode(ResponseModel apiModel)
    {
        return apiModel.Code != 0;
    }

    protected override string? GetErrorMessage(ResponseModel apiModel)
    {
        return apiModel.Message;
    }

    protected override object? GetContent(ResponseModel apiModel)
    {
        return apiModel.Data;
    }
}