namespace BilibiliClient.Core.Contracts.Services;

public interface IApiErrorCodeHandlerService
{
    /// <summary>
    /// 处理错误码
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    ValueTask HandlerApiError(long errorCode, string? errorMessage);
}