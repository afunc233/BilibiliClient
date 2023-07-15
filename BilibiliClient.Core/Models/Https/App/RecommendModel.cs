namespace BilibiliClient.Core.Models.Https.App;

public class RecommendModel
{
    /// <summary>
    /// 这个应该是控制数据的 实现类似翻页的操作
    /// </summary>
    public string? Idx { get; set; } = "0";

    /// <summary>
    /// 不知道为啥传 5 
    /// </summary>
    public string? Flush { get; set; } = "5";

    /// <summary>
    /// 不知道为啥传4
    /// </summary>
    public string? Column { get; set; } = "4";

    /// <summary>
    /// 
    /// </summary>
    public string? Device { get; set; } = "pad";

    public string? DeviceName { get; set; } = "iPad 6";

    public string? Pull { get; set; } = "0";
}