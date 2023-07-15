using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Configs;

public class AndroidPlatformConfig : IPlatformConfig
{
    public ApiPlatform ApiPlatform => ApiPlatform.Android;
    public string Platform => "android";
    public string AppKey => "4409e2ce8ffd12b8";
    public string AppSecret => "59b43e04ad6965f34319062b478f83dd";
    public string MobileApp => "android";
}