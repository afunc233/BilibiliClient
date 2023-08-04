using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Passport;

/**
     "data": {
        "api_host": "link.acg.tv",
        "has_login": 1,
        "direct_login": 0,
        "user_info": {
            "mid": 1602373316,
            "uname": "bili_45690362011",
            "face": "https://i2.hdslb.com/bfs/face/4bffa122dc57d8bbf3289acc8b48210d914e9a8f.jpg"
        },
        "confirm_uri": ""
    }
 */
public class UserInfo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("mid")]
    public int Mid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("uname")]
    public string? Uname { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("face")]
    public string? Face { get; set; }
}

public class LoginAppThirdResult
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("api_host")]
    public string? ApiHost { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("has_login")]
    public int has_login { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("direct_login")]
    public int direct_login { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("user_info")]
    public UserInfo? UserInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("confirm_uri")]
    public string? ConfirmUri { get; set; }
}