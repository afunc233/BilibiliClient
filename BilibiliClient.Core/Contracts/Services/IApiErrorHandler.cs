namespace BilibiliClient.Core.Contracts.Services;

/// <summary>
/// 错误码处理，分散开处理
/// </summary>
public interface IApiErrorHandler
{
    /// <summary>
    /// 是否能处理
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    bool CanHanded(long errorCode);

    /// <summary>
    /// 处理异常
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    Task<bool> HandError(long errorCode, string? errorMessage);
}