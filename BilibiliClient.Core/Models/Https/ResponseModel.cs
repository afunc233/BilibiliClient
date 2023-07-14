using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https;

public class ResponseModel
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("code")]
    public long Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("ttl")]
    public int Ttl { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("data")]
    public object? Data { get; set; }
}