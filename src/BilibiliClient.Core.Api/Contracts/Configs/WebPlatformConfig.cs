using BilibiliClient.Core.Api.Configs;

namespace BilibiliClient.Core.Api.Contracts.Configs;

internal class WebPlatformConfig : IPlatformConfig
{
    public ApiPlatform ApiPlatform => ApiPlatform.Web;
    public string Platform => "web";
    public string AppKey => "84956560bc028eb7";
    public string AppSecret => "94aba54af9065f71de72f5508f1cd42e";
}