using BilibiliClient.Core.Api.Configs;

namespace BilibiliClient.Core.Api.Contracts.Api;

internal interface IApi
{
    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="paramPairs"></param>
    /// <param name="apiPlatform"></param>
    /// <returns></returns>
    ValueTask SignParam(List<KeyValuePair<string, string>> paramPairs, ApiPlatform apiPlatform);
}