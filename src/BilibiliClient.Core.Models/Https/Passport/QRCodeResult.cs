using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Passport;

public class QRCodeResult
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    
    [JsonPropertyName("auth_code")]
    public string? AuthCode { get; set; }
}