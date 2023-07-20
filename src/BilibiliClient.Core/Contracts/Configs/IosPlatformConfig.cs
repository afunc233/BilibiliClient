using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Configs;

public class IosPlatformConfig : IPlatformConfig
{
    public ApiPlatform ApiPlatform => ApiPlatform.Ios;
    public string Platform => "ios";
    public string AppKey => "27eb53fc9058f8c3";
    public string AppSecret => "c2ed53a74eeefe3cf99fbd01d8c9c375";
    public string MobileApp => "iphone";

    public long GetNowMilliSeconds()
    {
        return DateTimeOffset.Now.ToLocalTime().ToUnixTimeSeconds();
    }
}