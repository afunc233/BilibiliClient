using System.Text.Json.Serialization;

namespace BilibiliClient.Core.Models.Https.Passport;

public class CountryOrRegion
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("cname")]
    public string? Cname { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("country_id")]
    public string? CId { get; set; }
}

public class CountryList
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("common")]
    public List<CountryOrRegion>? CommonList { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("others")]
    public List<CountryOrRegion>? OtherList { get; set; }
}