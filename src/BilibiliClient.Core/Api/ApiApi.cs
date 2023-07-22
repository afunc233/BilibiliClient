using BilibiliClient.Core.Configs;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Contracts.Configs;
using BilibiliClient.Core.Contracts.Models;
using BilibiliClient.Core.Models.Https.Api;

namespace BilibiliClient.Core.Api;

public class ApiApi : AbsApi, IApiApi
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
            new KeyValuePair<string, string>("fnver", "0"),
            new KeyValuePair<string, string>("cid", cId),
            new KeyValuePair<string, string>("fourk", "1"),
            new KeyValuePair<string, string>("fnval", "4048"),
            new KeyValuePair<string, string>("qn", "64"),
            new KeyValuePair<string, string>("otype", "json"),
            new KeyValuePair<string, string>("avid", avId),
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