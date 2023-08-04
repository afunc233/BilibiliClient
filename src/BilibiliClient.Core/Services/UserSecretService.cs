using System.Net;
using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Models;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Utils;
using BilibiliClient.Models.gRPC;

namespace BilibiliClient.Core.Services;

internal class UserSecretService : IUserSecretService
{
    private readonly UserSecretConfig _userSecretConfig;

    private readonly IJsonFileService _jsonFileService;

    private readonly ICookieService _cookieService;

    public UserSecretService(IJsonFileService jsonFileService,
        UserSecretConfig userSecretConfig, ICookieService cookieService)
    {
        _jsonFileService = jsonFileService;
        _userSecretConfig = userSecretConfig;
        _cookieService = cookieService;
    }

    public async Task LoadUserSecret()
    {
        await Task.CompletedTask;
        var localValue =
            _jsonFileService.Read<UserSecretConfig>(".", nameof(UserSecretConfig), s => DESUtil.Decrypt(s));

        if (localValue != null)
        {
            _userSecretConfig.UserId = localValue.UserId;
            _userSecretConfig.AccessToken = localValue.AccessToken;
            _userSecretConfig.AccessKey = localValue.AccessKey;
            _userSecretConfig.RefreshToken = localValue.RefreshToken;
            _userSecretConfig.ExpiresIn = localValue.ExpiresIn;
            _userSecretConfig.LastSaveAuthTime = localValue.LastSaveAuthTime;
            _userSecretConfig.DomainList = localValue.DomainList;
            _userSecretConfig.CookieList = localValue.CookieList;
            _userSecretConfig.Buvid = localValue.Buvid ??= GetBuvid();
        }

        if (_userSecretConfig is { DomainList: not null } and { CookieList: not null })
        {
            await _cookieService.LoadCookie(_userSecretConfig.DomainList, (domain =>
            {
                var cookieCollection = new CookieCollection();
                foreach (var cookieItem in _userSecretConfig.CookieList)
                {
                    var cookie = new Cookie()
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
            }));
        }
    }

    public async Task SaveUserSecret(UserSecretConfig? userSecretConfig)
    {
        await Task.CompletedTask;
        userSecretConfig ??= _userSecretConfig;
        _jsonFileService.Save(".", nameof(UserSecretConfig), userSecretConfig, s => DESUtil.Encrypt(s));
    }


    private static string GetBuvid()
    {
        var macAddress = Guid.NewGuid().ToString();
        var buvidObj = new Buvid(macAddress);
        return buvidObj.Generate();
    }
}