using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Contracts.Api;

public interface IApi
{
    /// <summary>
    /// 签名
    /// </summary>
    /// <param name="paramPairs"></param>
    /// <param name="apiPlatform"></param>
    /// <returns></returns>
    ValueTask SignParam(List<KeyValuePair<string, string>> paramPairs, ApiPlatform apiPlatform);
}