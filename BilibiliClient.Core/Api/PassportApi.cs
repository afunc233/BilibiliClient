using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Configs;
using BilibiliClient.Core.Models.Https.Passport;

namespace BilibiliClient.Core.Api;

public class PassportApi : AbsApi, IPassportApi
{
    private readonly IPassportHttpClient _passportHttpClient;

    public PassportApi(IPassportHttpClient passportHttpClient, IEnumerable<IPlatformConfig> platformConfigs)
        : base(platformConfigs)
    {
        _passportHttpClient = passportHttpClient;
    }

    /// <summary>
    /// 后续看看怎么使用  https://github.com/kuresaru/geetest-validator  
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public async ValueTask<LoginCaptcha?> LoginCaptcha(string source = "main_web")
    {
        const string url = "/x/passport-login/captcha";

        var paramsList = new List<KeyValuePair<string, string>>()
        {
            new(nameof(source), source)
        };
        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Get, paramsList);
        return await _passportHttpClient.SendAsync<LoginCaptcha>(request);
    }

    public async ValueTask<CountryList?> CountryList()
    {
        const string url = "/web/generic/country/list";
        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Get);

        return await _passportHttpClient.SendAsync<CountryList>(request);
    }

    public async ValueTask<object?> SendSms(SendSmsModel sendSmsModel)
    {
        const string url = "/x/passport-login/sms/send";

        var paramsList = new List<KeyValuePair<string, string?>>()
        {
            new KeyValuePair<string, string?>("cid", sendSmsModel.Cid.ToString()),
            new KeyValuePair<string, string?>("tel", sendSmsModel.Tel.ToString()),
            new KeyValuePair<string, string?>("login_session_id", sendSmsModel.LoginSessionId),
            new KeyValuePair<string, string?>("recaptcha_token", sendSmsModel.RecaptchaToken),
            new KeyValuePair<string, string?>("gee_challenge", sendSmsModel.GeeChallenge),
            new KeyValuePair<string, string?>("gee_validate", sendSmsModel.GeeValidate),
            new KeyValuePair<string, string?>("gee_seccode", sendSmsModel.GeeSeccode),
            new KeyValuePair<string, string?>("channel", sendSmsModel.Channel),
            new KeyValuePair<string, string?>("buvid", sendSmsModel.Buvid),
            new KeyValuePair<string, string?>("local_id", sendSmsModel.LocalId),
            new KeyValuePair<string, string?>("statistics", sendSmsModel.Statistics),
        };
        using var httpContent = new FormUrlEncodedContent(paramsList);

        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Post, null, httpContent);
        return await _passportHttpClient.SendAsync<object>(request);
    }

    public async ValueTask<object?> LoginSms()
    {
        const string url = "/x/passport-login/login/sms";

        var paramsList = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("cid", "")
        };
        using var httpContent = new FormUrlEncodedContent(paramsList);

        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Post, null, httpContent);
        return await _passportHttpClient.SendAsync<object>(request);
    }


    public async ValueTask<bool> CheckToken(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return false;
        }

        await Task.CompletedTask;

        const string url = "/api/oauth2/info";
        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("access_token", accessToken)
        };

        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Get, queryParameters);
        var aa = await _passportHttpClient.SendAsync<object>(request);

        return false;
    }

    public async ValueTask<object?> RefreshToken()
    {
        await Task.CompletedTask;
        // const string url = "/api/oauth2/refreshToken";

        return null;
    }
}