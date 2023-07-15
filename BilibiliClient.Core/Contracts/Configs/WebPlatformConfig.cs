using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Configs;

public class WebPlatformConfig : IPlatformConfig
{
    public ApiPlatform ApiPlatform => ApiPlatform.Web;
    public string Platform => "web";
    public string AppKey => "84956560bc028eb7";
    public string AppSecret => "94aba54af9065f71de72f5508f1cd42e";
}