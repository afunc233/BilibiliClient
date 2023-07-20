using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Passport;

public class TokenInfo
{
    /// <summary>
    /// "mid": 1602373316,
    /// </summary>

    [JsonPropertyName("mid")]
    public long Mid { get; set; }

    /// <summary>
    /// "access_token": "ae630a4d0ec12b26ff870562d1e38872",
    /// </summary>

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    ///          "refresh_token": "63e0b5d576e60755bb3a76e9c146b872",
    /// </summary>

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// "expires_in": 15552000,
    /// </summary>

    [JsonPropertyName("expires_in")]
    public long expires_in { get; set; }
}

public class CookieInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("cookies")]
    public List<Cookie>? CookieList { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("domains")]
    public List<string>? DomainList { get; set; }
}

public class Cookie
{
    /// <summary>
    ///                      "name": "SESSDATA",
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// "value": "a67fd800%2C1705303754%2C2f4dbe72",
    /// </summary>

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// "http_only": 1,
    /// </summary>
    [JsonPropertyName("http_only")]
    public int HttpOnly { get; set; }

    /// <summary>
    /// "expires": 1705303754,
    /// </summary>

    [JsonPropertyName("expires")]
    public long Expires { get; set; }

    /// <summary>
    /// "secure": 0
    /// </summary>

    [JsonPropertyName("secure")]
    public int Secure { get; set; }
}

public class QRCodePollResult
{
    /// <summary>
    ///  "is_new": false,
    /// </summary>
    [JsonPropertyName("is_new")]
    public bool IsNew { get; set; }

    /// <summary>
    /// "mid": 1602373316,
    /// </summary>

    [JsonPropertyName("mid")]
    public long Mid { get; set; }

    /// <summary>
    /// "access_token": "ae630a4d0ec12b26ff870562d1e38872",
    /// </summary>

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    ///          "refresh_token": "63e0b5d576e60755bb3a76e9c146b872",
    /// </summary>

    [JsonPropertyName("refresh_token")]
    public string? refresh_token { get; set; }

    /// <summary>
    ///          "expires_in": 15552000,
    /// </summary>

    [JsonPropertyName("expires_in")]
    public long expires_in { get; set; }

    /// <summary>
    ///          "token_info": {},
    /// </summary>

    [JsonPropertyName("token_info")]
    public TokenInfo? TokenInfo { get; set; }

    /// <summary>
    /// "cookie_info": {}
    /// </summary>
    [JsonPropertyName("cookie_info")]
    public CookieInfo? CookieInfo { get; set; }

    /// <summary>
    /// "sso": []
    /// </summary>
    [JsonPropertyName("sso")]
    public List<string>? sso { get; set; }

    /// <summary>
    ///  "hint": ""
    /// </summary>
    [JsonPropertyName("hint")]
    public string? Hint { get; set; }
}