using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Configs;

public interface IPlatformConfig
{
    ApiPlatform ApiPlatform { get; }

    /// <summary>
    /// 平台
    /// </summary>
    string Platform { get; }

    /// <summary>
    /// 签名的 Key 
    /// </summary>
    string AppKey { get; }

    /// <summary>
    /// 签名的 Secret
    /// </summary>
    string AppSecret { get; }

    /// <summary>
    /// 设备
    /// </summary>
    string Device
    {
        get { return "phone"; }
    }

    /// <summary>
    /// 移动端的话，具体是什么端,为空的话 不必参与签名
    /// </summary>
    string MobileApp
    {
        get { return ""; }
    }

    long GetNowMilliSeconds()
    {
        return DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds();
    }
}