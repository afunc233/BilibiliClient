using System.Security.Cryptography;
using System.Text;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Configs;
using BilibiliClient.Core.Contracts.Models;

namespace BilibiliClient.Core.Api;

/// <summary>
/// 抽象的Api 目前计划是给参数签名的东西放到这个里面
/// </summary>
public abstract class AbsApi : IApi
{
    private readonly IEnumerable<IPlatformConfig> _platformConfigs;

    protected AbsApi(IEnumerable<IPlatformConfig> platformConfigs)
    {
        _platformConfigs = platformConfigs;
    }

    public virtual async ValueTask SignParam(List<KeyValuePair<string, string>> paramPairs,
        ApiPlatform apiPlatform)
    {
        await Task.CompletedTask;
        var platformConfig = _platformConfigs.FirstOrDefault(it => it.ApiPlatform == apiPlatform);
        if (platformConfig == null)
        {
            return;
        }

        paramPairs.Add(new KeyValuePair<string, string>("ts", platformConfig.GetNowMilliSeconds().ToString()));
        paramPairs.Add(new KeyValuePair<string, string>("appkey", platformConfig.AppKey));
        if (string.IsNullOrWhiteSpace(platformConfig.Platform))
            paramPairs.Add(new KeyValuePair<string, string>("platform", platformConfig.Platform));
        if (string.IsNullOrWhiteSpace(platformConfig.MobileApp))
            paramPairs.Add(new KeyValuePair<string, string>("mobi_app", platformConfig.MobileApp));

        // 序列化参数
        StringBuilder queryBuilder = new StringBuilder();
        foreach (var entry in paramPairs.OrderBy(it => it.Key))
        {
            if (queryBuilder.Length > 0)
            {
                queryBuilder.Append('&');
            }

            queryBuilder.Append(Uri.EscapeDataString(entry.Key))
                .Append('=')
                .Append(Uri.EscapeDataString(entry.Value));
        }

        var md5 = GenerateMd5(queryBuilder.Append(platformConfig.AppSecret).ToString());
        paramPairs.Add(new KeyValuePair<string, string>("sign", md5));
    }

    private static String GenerateMd5(String input)
    {
        byte[] digest = MD5.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder sb = new StringBuilder();
        foreach (byte b in digest)
        {
            sb.Append($"{b:x2}");
        }

        return sb.ToString();
    }
}