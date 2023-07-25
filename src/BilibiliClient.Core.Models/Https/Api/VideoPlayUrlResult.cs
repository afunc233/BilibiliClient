using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Api;

public class VideoPlayUrlDashVideo
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("baseUrl")]
    public string? BaseUrl { get; set; }
}

public class VideoPlayUrlDashAudio
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("baseUrl")]
    public string? BaseUrl { get; set; }
}

public class VideoPlayUrlDash
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("video")]
    public List<VideoPlayUrlDashVideo>? Video { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("audio")]
    public List<VideoPlayUrlDashAudio>? Audio { get; set; }
}

public class VideoPlayUrlResult
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("dash")]
    public VideoPlayUrlDash? Dash { get; set; }
}