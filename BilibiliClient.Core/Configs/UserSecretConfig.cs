using BilibiliClient.Core.Models.Https.Passport;

namespace BilibiliClient.Core.Configs;

public class UserSecretConfig
{
    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public string? UserId { get; set; }

    public long ExpiresIn { get; set; }

    public long LastSaveAuthTime { get; set; }
    
    
    public List<string>? DomainList { get; set; }

    public List<Cookie>? CookieList { get; set; }
}