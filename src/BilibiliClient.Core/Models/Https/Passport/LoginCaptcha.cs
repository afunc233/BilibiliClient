using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Passport;

public class Geetest
{
    [JsonPropertyName("challenge")]
    public string? Challenge { get; set; }

    [JsonPropertyName("gt")]
    public string? Gt { get; set; }
}

public class Tencent
{
    [JsonPropertyName("appid")]
    public string? AppId { get; set; }
}

public class LoginCaptcha
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("token")]

    public string? Token { get; set; }
    [JsonPropertyName("geetest")]

    public Geetest? Geetest { get; set; }
    [JsonPropertyName("tencent")]

    public Tencent? Tencent { get; set; }
}