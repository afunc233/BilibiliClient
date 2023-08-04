using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Configs;
using BilibiliClient.Core.Api.Models;
using BilibiliClient.Core.Models.Https.Api;

namespace BilibiliClient.Core.Api;

internal class ApiApi : AbsApi, IApiApi
{
    private readonly IApiHttpClient _apiHttpClient;
    private readonly UserSecretConfig _userSecretConfig;

    public ApiApi(IApiHttpClient apiHttpClient, IEnumerable<IPlatformConfig> platformConfigs,
        UserSecretConfig userSecretConfig) : base(
        platformConfigs)
    {
        _apiHttpClient = apiHttpClient;
        _userSecretConfig = userSecretConfig;
    }

    public async ValueTask<VideoPlayUrlResult?> GetVideoPlayUrl(string avId, string cId)
    {
        const string url = "/x/player/playurl";

        var queryParameters = new List<KeyValuePair<string, string>>()
        {
            new("fnver", "0"),
            new("cid", cId),
            new("fourk", "1"),
            new("fnval", "4048"),
            new("qn", "64"),
            new("otype", "json"),
            new("avid", avId),
        };
        if (!string.IsNullOrWhiteSpace(_userSecretConfig.UserId))
        {
            queryParameters.Add(new KeyValuePair<string, string>("mid", _userSecretConfig.UserId));
        }

        await SignParam(queryParameters, ApiPlatform.Ios);

        var request = await _apiHttpClient.BuildRequestMessage(url, HttpMethod.Get, queryParameters);

        return await _apiHttpClient.SendAsync<VideoPlayUrlResult>(request);
    }
}