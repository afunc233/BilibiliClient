using System.Net;
using System.Web;
using BilibiliClient.Core.ApiHttpClient;
using BilibiliClient.Core.Configs;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using BilibiliClient.Core.Models.Https.Passport;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.Core.Services;

public class LoginService : ILoginService
{
    private string? _loginId;
    private string? _authCode;
    private readonly IPassportApi _passportApi;

    private readonly ICookieService _cookieService;
    private readonly UserSecretConfig _userSecretConfig;

    private readonly IMessenger _messenger;

    public LoginService(IMessenger messenger, IPassportApi passportApi, ICookieService cookieService,
        UserSecretConfig userSecretConfig)
    {
        _messenger = messenger;
        _passportApi = passportApi;
        _cookieService = cookieService;
        _userSecretConfig = userSecretConfig;
    }

    public async Task<string?> GetLoginQRCode()
    {
        _loginId = Guid.NewGuid().ToString();

        var qrCodeAuthCode = await _passportApi.QRCodeAuthCode(_loginId);
        if (qrCodeAuthCode == null)
        {
            return null;
        }

        _authCode = qrCodeAuthCode.AuthCode;
        return qrCodeAuthCode.Url;
    }


    public async Task CheckHasLogin()
    {
        if (string.IsNullOrWhiteSpace(_loginId))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_authCode))
        {
            return;
        }

        await Task.CompletedTask;

        var qrCodePollResult = await _passportApi.QRCodePoll(_loginId, _authCode);
        if (qrCodePollResult?.CookieInfo is null || qrCodePollResult?.TokenInfo is null)
        {
            return;
        }

        _messenger.Send(new LoginStateMessage(LoginStateEnum.StopQRCodePoll));
        await SaveCookie(qrCodePollResult.CookieInfo);

        var loginAppThirdResult = await _passportApi.LoginAppThird();

        if (string.IsNullOrWhiteSpace(loginAppThirdResult?.ConfirmUri))
        {
            _messenger.Send(new LoginStateMessage(LoginStateEnum.Fail));
            return;
        }

        var accessKey = await _passportApi.GetAccessKey(loginAppThirdResult.ConfirmUri);

        if (string.IsNullOrWhiteSpace(accessKey))
        {
            _messenger.Send(new LoginStateMessage(LoginStateEnum.Fail));
            return;
        }

        qrCodePollResult.AccessToken = accessKey;

        _userSecretConfig.AccessToken = accessKey;
        _userSecretConfig.UserId = qrCodePollResult.Mid.ToString();
        _userSecretConfig.RefreshToken = qrCodePollResult.refresh_token;
        _userSecretConfig.ExpiresIn = qrCodePollResult.expires_in;
        _userSecretConfig.LastSaveAuthTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        _userSecretConfig.DomainList = qrCodePollResult.CookieInfo.DomainList;
        _userSecretConfig.CookieList = qrCodePollResult.CookieInfo.CookieList;

        _messenger.Send(new LoginStateMessage(LoginStateEnum.Success));
    }

    private async ValueTask SaveCookie(CookieInfo? cookieInfo)
    {
        if (cookieInfo is { CookieList: not null } and { DomainList: not null })
        {
            await _cookieService.LoadCookie(cookieInfo.DomainList, (domain) =>
            {
                var cookieCollection = new CookieCollection();
                foreach (var cookieItem in cookieInfo.CookieList)
                {
                    var cookie = new System.Net.Cookie()
                    {
                        Domain = domain,
                        Name = cookieItem.Name ?? "",
                        Value = cookieItem.Value,
                        HttpOnly = cookieItem.HttpOnly == 1,
                        Secure = cookieItem.Secure == 1,
                        Expires = DateTimeOffset.FromUnixTimeSeconds(cookieItem.Expires).UtcDateTime,
                        Path = "/"
                    };

                    cookieCollection.Add(cookie);
                }

                return cookieCollection;
            });
        }
    }
}