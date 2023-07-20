using System.Web;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Configs;
using BilibiliClient.Core.Contracts.Models;
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

    #region 好像目前用处不大

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

    #endregion

    #region 登录相关

    public async ValueTask<TokenInfo?> CheckToken(string? accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return default;
        }

        await Task.CompletedTask;

        const string url = "/api/oauth2/info";

        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("access_token", accessToken),
            new KeyValuePair<string, string>("access_key", accessToken),
        };

        var query = await SignParamQueryString(queryParameters, ApiPlatform.Android);

        var request = await _passportHttpClient.BuildRequestMessage($"{url}?{query}", HttpMethod.Get);
        return await _passportHttpClient.SendAsync<TokenInfo>(request);
    }

    public async ValueTask<TokenInfo?> RefreshToken(string? accessToken, string? refreshToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return default;
        }

        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("access_token", accessToken),
            new KeyValuePair<string, string>("access_key", accessToken),
            new KeyValuePair<string, string>("refresh_token", refreshToken),
        };

        const string url = "/api/oauth2/refreshToken";

        await SignParam(queryParameters, ApiPlatform.Android);
        using var formUrlEncodedContent = new FormUrlEncodedContent(queryParameters);
        var request =
            await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Post, httpContent: formUrlEncodedContent);
        return await _passportHttpClient.SendAsync<TokenInfo>(request);
    }

    public async ValueTask<QRCodeResult?> QRCodeAuthCode(string localId)
    {
        await Task.CompletedTask;
        const string url = "x/passport-tv-login/qrcode/auth_code";

        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("local_id", localId)
        };

        await SignParam(queryParameters, ApiPlatform.Android);

        using var formUrlEncodedContent = new FormUrlEncodedContent(queryParameters);

        var request =
            await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Post, httpContent: formUrlEncodedContent);

        return await _passportHttpClient.SendAsync<QRCodeResult>(request);
    }

    public async ValueTask<QRCodePollResult?> QRCodePoll(string localId, string authCode)
    {
        await Task.CompletedTask;
        const string url = "x/passport-tv-login/qrcode/poll";

        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("local_id", localId),
            new KeyValuePair<string, string>("auth_code", authCode),
        };

        await SignParam(queryParameters, ApiPlatform.Tv);

        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Post, queryParameters);

        return await _passportHttpClient.SendAsync<QRCodePollResult>(request);
    }

    public async ValueTask<LoginAppThirdResult?> LoginAppThird()
    {
        const string url = "/login/app/third";
        const string loginAppThirdApi = "http://link.acg.tv/forum.php";

        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("api", loginAppThirdApi),
        };

        await SignBeforeAppKey(queryParameters, ApiPlatform.Ios);

        var request = await _passportHttpClient.BuildRequestMessage(url, HttpMethod.Get, queryParameters);

        return await _passportHttpClient.SendAsync<LoginAppThirdResult>(request);
    }

    public async ValueTask<string?> GetAccessKey(string confirmUri)
    {
        await Task.CompletedTask;

        var request = await _passportHttpClient.BuildRequestMessage(confirmUri, HttpMethod.Get);

        var response = await _passportHttpClient.Send4ResponseAsync(request);
        var success = response.Headers.TryGetValues("location", out var locations);
        if (!success)
        {
            return default;
        }

        var redirectUrl = locations?.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(redirectUrl))
        {
            return default;
        }

        var uri = new Uri(redirectUrl);
        var queries = HttpUtility.ParseQueryString(uri.Query);
        var accessKey = queries.Get("access_key");
        return accessKey;
    }

    #endregion
}