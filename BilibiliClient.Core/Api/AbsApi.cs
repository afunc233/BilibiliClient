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

    protected virtual async ValueTask<string> SignParamQueryString(
        List<KeyValuePair<string, string>>? paramPairs = null,
        ApiPlatform apiPlatform = ApiPlatform.Ios)
    {
        paramPairs ??= new List<KeyValuePair<string, string>>();

        paramPairs.Add(new KeyValuePair<string, string>("build", "5520400"));

        var platformConfig = await AddParams(paramPairs, apiPlatform);
        var sign = await Sign(paramPairs, platformConfig);
        paramPairs.Add(new KeyValuePair<string, string>("sign", sign));
        var queryList = paramPairs.Select(p => $"{p.Key}={p.Value}").ToList();
        queryList.Sort();
        var query = string.Join('&', queryList);
        return query;
    }

    protected virtual async ValueTask<IPlatformConfig> AddParams(List<KeyValuePair<string, string>> paramPairs,
        ApiPlatform apiPlatform)
    {
        await Task.CompletedTask;
        var platformConfig = _platformConfigs.FirstOrDefault(it => it.ApiPlatform == apiPlatform);
        if (platformConfig == null)
        {
            return _platformConfigs.First();
        }

        paramPairs.Add(new KeyValuePair<string, string>("ts", platformConfig.GetNowMilliSeconds().ToString()));
        paramPairs.Add(new KeyValuePair<string, string>("appkey", platformConfig.AppKey));
        if (!string.IsNullOrWhiteSpace(platformConfig.Platform))
            paramPairs.Add(new KeyValuePair<string, string>("platform", platformConfig.Platform));
        if (!string.IsNullOrWhiteSpace(platformConfig.MobileApp))
            paramPairs.Add(new KeyValuePair<string, string>("mobi_app", platformConfig.MobileApp));
        if (!string.IsNullOrWhiteSpace(platformConfig.Device))
            paramPairs.Add(new KeyValuePair<string, string>("device", platformConfig.Device));
        return platformConfig;
    }

    protected virtual async ValueTask<string> Sign(List<KeyValuePair<string, string>> paramPairs,
        IPlatformConfig platformConfig)
    {
        await Task.CompletedTask;
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

        return GenerateMd5(queryBuilder.Append(platformConfig.AppSecret).ToString());
    }

    public virtual async ValueTask SignParam(List<KeyValuePair<string, string>> paramPairs,
        ApiPlatform apiPlatform)
    {
        await Task.CompletedTask;
        var platformConfig = await AddParams(paramPairs, apiPlatform);

        var sign = await Sign(paramPairs, platformConfig);
        paramPairs.Add(new KeyValuePair<string, string>("sign", sign));
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