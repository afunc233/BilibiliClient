using BilibiliClient.Core.Models.Https.Passport;

namespace BilibiliClient.Core.Contracts.Api;

public interface IAccountApi
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
}