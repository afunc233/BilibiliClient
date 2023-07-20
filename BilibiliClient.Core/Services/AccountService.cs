using System.Net;
using BilibiliClient.Core.Configs;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using BilibiliClient.Core.Models.Https.Passport;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.Core.Services;

public class AccountService : IAccountService
{
    private string? _loginId;
    private string? _authCode;
    private readonly IPassportApi _passportApi;

    private readonly ICookieService _cookieService;
    private readonly UserSecretConfig _userSecretConfig;

    private readonly IMessenger _messenger;

    public AccountService(IMessenger messenger, IPassportApi passportApi, ICookieService cookieService,
        UserSecretConfig userSecretConfig)
    {
        _messenger = messenger;
        _passportApi = passportApi;
        _cookieService = cookieService;
        _userSecretConfig = userSecretConfig;
    }

    public async Task<string?> GetLoginQRCode()
    {
        _loginId = Guid.NewGuid().ToString("N");

        var qrCodeAuthCode = await _passportApi.QRCodeAuthCode(_loginId);
        if (qrCodeAuthCode == null)
        {
            return null;
        }

        _authCode = qrCodeAuthCode.AuthCode;
        return qrCodeAuthCode.Url;
    }


    public async Task CheckLoginState()
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
        if (qrCodePollResult?.CookieInfo is null || qrCodePollResult.TokenInfo is null)
        {
            return;
        }

        _messenger.Send(new LoginStateMessage(LoginStateEnum.StopQRCodePoll));
        await SaveCookie(qrCodePollResult.CookieInfo);

        _userSecretConfig.AccessToken = qrCodePollResult.AccessToken;
        _userSecretConfig.UserId = qrCodePollResult.Mid.ToString();
        _userSecretConfig.RefreshToken = qrCodePollResult.refresh_token;
        _userSecretConfig.ExpiresIn = qrCodePollResult.expires_in;
        _userSecretConfig.LastSaveAuthTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        _userSecretConfig.DomainList = qrCodePollResult.CookieInfo.DomainList;
        _userSecretConfig.CookieList = qrCodePollResult.CookieInfo.CookieList;

        _messenger.Send(new SaveUserSecretMessage());
        _messenger.Send(new LoginStateMessage(LoginStateEnum.LoginSuccess));
    }

    public async Task<bool> IsLocalTokenValid(bool forceVerify = false)
    {
        if (string.IsNullOrWhiteSpace(_userSecretConfig.AccessToken))
        {
            return false;
        }

        if (_userSecretConfig.LastSaveAuthTime <= 0 || _userSecretConfig.ExpiresIn <= 0)
        {
            return false;
        }

        var lastAuthorizeTime = DateTimeOffset.FromUnixTimeSeconds(_userSecretConfig.LastSaveAuthTime);

        var offsetTime = DateTimeOffset.Now - lastAuthorizeTime;

        if (offsetTime.TotalSeconds > _userSecretConfig.ExpiresIn)
        {
            return false;
        }

        if (forceVerify)
        {
            var tokenInfo = await _passportApi.CheckToken(_userSecretConfig.AccessToken);
            if (tokenInfo == null)
            {
                return false;
            }
            _userSecretConfig.ExpiresIn = tokenInfo.expires_in;
        }

        return true;
    }

    public async Task<bool> RefreshToken()
    {
        await Task.CompletedTask;

        var tokenInfo = await _passportApi.RefreshToken(_userSecretConfig.AccessToken, _userSecretConfig.RefreshToken);

        if (tokenInfo == null)
        {
            return false;
        }

        _userSecretConfig.AccessToken = tokenInfo.AccessToken;
        _userSecretConfig.RefreshToken = tokenInfo.RefreshToken;
        _userSecretConfig.LastSaveAuthTime = DateTimeOffset.Now.ToUnixTimeSeconds();

        _messenger.Send(new SaveUserSecretMessage());
        return true;
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