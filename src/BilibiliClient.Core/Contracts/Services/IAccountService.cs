namespace BilibiliClient.Core.Contracts.Services;

public interface IAccountService
{
    /// <summary>
    /// 弄个二维码，准备登录
    /// </summary>
    /// <returns></returns>
    Task<string?> GetLoginQRCode();

    /// <summary>
    /// 检测二维码目前的状态
    /// </summary>
    /// <returns></returns>
    Task CheckLoginState();

    /// <summary>
    /// Token 是否是有效的
    /// </summary>
    /// <returns></returns>
    Task<bool> IsLocalTokenValid(bool forceVerify = false);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<bool> RefreshToken();

    Task<object?> GetMyInfo();
}