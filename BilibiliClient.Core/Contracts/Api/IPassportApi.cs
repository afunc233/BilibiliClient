using BilibiliClient.Core.Models.Https.Passport;

namespace BilibiliClient.Core.Contracts.Api;

public interface IPassportApi
{
    /// <summary>
    /// https://socialsisteryi.github.io/bilibili-API-collect/docs/login/login_action/
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    ValueTask<LoginCaptcha?> LoginCaptcha(string source = "main_web");

    /// <summary>
    /// https://socialsisteryi.github.io/bilibili-API-collect/docs/login/login_action/SMS.html#%E8%8E%B7%E5%8F%96%E5%9B%BD%E9%99%85%E5%86%A0%E5%AD%97%E7%A0%81-web%E7%AB%AF
    /// </summary>
    /// <returns></returns>
    ValueTask<CountryList?> CountryList();

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="sendSmsModel"></param>
    /// <returns></returns>
    ValueTask<object?> SendSms(SendSmsModel sendSmsModel);

    /// <summary>
    /// 手机号验证码登录
    /// </summary>
    /// <returns></returns>
    ValueTask<object?> LoginSms();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    ValueTask<TokenInfo?> CheckToken(string? accessToken);

    /// <summary>
    /// 目前这个接口不可用  refresh_token not match
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    ValueTask<TokenInfo?> RefreshToken(string? accessToken, string? refreshToken);


    /// <summary>
    /// 获取登录二维码
    /// </summary>
    /// <param name="localId"></param>
    /// <returns></returns>
    ValueTask<QRCodeResult?> QRCodeAuthCode(string localId);


    /// <summary>
    /// 轮询二维码扫码状态
    /// </summary>
    /// <param name="localId"></param>
    /// <param name="authCode"></param>
    /// <returns></returns>
    ValueTask<QRCodePollResult?> QRCodePoll(string localId, string authCode);

    /// <summary>
    /// cookie转访问令牌.    这里是用 token 获取 key  但 key 不是 token
    /// </summary>
    /// <returns></returns>
    ValueTask<LoginAppThirdResult?> LoginAppThird();


    /// <summary>
    ///  这里是用 token 获取 key  但 key 不是 token
    /// </summary>
    /// <param name="confirmUri">LoginAppThird 接口返回的 confirmUri</param>
    /// <returns></returns>
    ValueTask<string?> GetAccessKey(string confirmUri);
}