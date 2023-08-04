using System.Security.Cryptography;
using System.Text;
using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.Configs;

namespace BilibiliClient.Core.Api;

/// <summary>
/// 抽象的Api 目前计划是给参数签名的东西放到这个里面
/// </summary>
internal abstract class AbsApi : IApi
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

        var platformConfig = await AddPlatformParams(paramPairs, apiPlatform);
        var sign = await GenSign(paramPairs, platformConfig);
        paramPairs.Add(new KeyValuePair<string, string>("sign", sign));
        var queryList = paramPairs.Select(p => $"{p.Key}={p.Value}").ToList();
        queryList.Sort();
        var query = string.Join('&', queryList);
        return query;
    }

    private IPlatformConfig GetPlatformConfig(ApiPlatform apiPlatform)
    {
        var platformConfig = _platformConfigs.FirstOrDefault(it => it.ApiPlatform == apiPlatform) ??
                             _platformConfigs.First();
        return platformConfig;
    }

    protected virtual async ValueTask<IPlatformConfig> AddPlatformParams(List<KeyValuePair<string, string>> paramPairs,
        ApiPlatform apiPlatform, bool justAppkey = false)
    {
        await Task.CompletedTask;
        var platformConfig = GetPlatformConfig(apiPlatform);

        paramPairs.Add(new KeyValuePair<string, string>("appkey", platformConfig.AppKey));
        if (!justAppkey)
        {
            paramPairs.Add(new KeyValuePair<string, string>("ts", platformConfig.GetNowMilliSeconds().ToString()));
            if (!string.IsNullOrWhiteSpace(platformConfig.Platform))
                paramPairs.Add(new KeyValuePair<string, string>("platform", platformConfig.Platform));
            if (!string.IsNullOrWhiteSpace(platformConfig.MobileApp))
                paramPairs.Add(new KeyValuePair<string, string>("mobi_app", platformConfig.MobileApp));
            // if (!string.IsNullOrWhiteSpace(platformConfig.Device))
            //     paramPairs.Add(new KeyValuePair<string, string>("device", platformConfig.Device));
        }

        return platformConfig;
    }

    /// <summary>
    /// 先加密，然后在加 Appkey
    /// </summary>
    /// <param name="queryParameters"></param>
    /// <param name="apiPlatform"></param>
    protected virtual async ValueTask SignBeforeAppKey(List<KeyValuePair<string, string>> queryParameters,
        ApiPlatform apiPlatform)
    {
        var sign = await GenSign(queryParameters, GetPlatformConfig(apiPlatform));

        queryParameters.Add(new KeyValuePair<string, string>("sign", sign));

        await AddPlatformParams(queryParameters, apiPlatform, true);
    }

    protected virtual async ValueTask<string> GenSign(List<KeyValuePair<string, string>> paramPairs,
        IPlatformConfig platformConfig)
    {
        await Task.CompletedTask;
        // 序列化参数
        StringBuilder queryBuilder = new();
        foreach (var entry in paramPairs.OrderBy(it => it.Key))
        {
            if (queryBuilder.Length > 0)
            {
                queryBuilder.Append('&');
            }

            queryBuilder.Append(Uri.EscapeDataString(entry.Key))
                .Append('=')
                .Append(entry.Value);
        }

        return GenerateMd5(queryBuilder.Append(platformConfig.AppSecret).ToString());
    }

    public virtual async ValueTask SignParam(List<KeyValuePair<string, string>> paramPairs,
        ApiPlatform apiPlatform)
    {
        await Task.CompletedTask;
        paramPairs.Add(new KeyValuePair<string, string>("build", "5520400"));
        var platformConfig = await AddPlatformParams(paramPairs, apiPlatform);

        var sign = await GenSign(paramPairs, platformConfig);
        paramPairs.Add(new KeyValuePair<string, string>("sign", sign));
    }


    private static string GenerateMd5(string input)
    {
        byte[] digest = MD5.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder sb = new();
        foreach (byte b in digest)
        {
            sb.Append($"{b:x2}");
        }

        return sb.ToString();
    }
}