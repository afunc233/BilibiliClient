using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Passport;

public class SendSmsModel
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("cid")]
    public int Cid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("tel")]
    public int Tel { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("login_session_id")]
    public string? LoginSessionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("recaptcha_token")]
    public string? RecaptchaToken { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gee_challenge")]
    public string? GeeChallenge { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gee_validate")]
    public string? GeeValidate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gee_seccode")]
    public string? GeeSeccode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("channel")]
    public string? Channel { get; set; } = "bili";

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("buvid")]
    public string? Buvid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("local_id")]
    public string? LocalId => Buvid;

    [JsonPropertyName("statistics")]
    public string Statistics
    {
        get => Uri.EscapeDataString("{\"appId\":1,\"platform\":3,\"version\":\"7.27.0\",\"abtest\":\"\"}");
    }
}