using BilibiliClient.Core.Models.Https.Passport;

namespace BilibiliClient.Core.Configs;

/// <summary>
/// 增加字段需要知道什么地方读取的，莫要用反射，等下 Aot 之后 反射可能就没有用了
/// </summary>
public class UserSecretConfig
{
    public string? AccessKey { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public string? UserId { get; set; }

    public long ExpiresIn { get; set; }

    public long LastSaveAuthTime { get; set; }


    public List<string>? DomainList { get; set; }

    public List<Cookie>? CookieList { get; set; }
}