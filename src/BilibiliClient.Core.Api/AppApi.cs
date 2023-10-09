using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Configs;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Api;

internal class AppApi(IAppHttpClient appHttpClient, IEnumerable<IPlatformConfig> platformConfigs) : AbsApi(platformConfigs), IAppApi
{
    private readonly IAppHttpClient _appHttpClient = appHttpClient;

    public async ValueTask<object?> SearchSquare(int from, int limit)
    {
        var url = "/x/v2/search/square";

        var paramList = new List<KeyValuePair<string, string>>()
        {
            new("device", "phone"),
            new("from", "0"),
            new("limit", "50"),
        };
        var query = await SignParamQueryString(paramList);
        var request = await _appHttpClient.BuildRequestMessage(url + $"?{query}", HttpMethod.Get);

        return await _appHttpClient.SendAsync<object>(request);
    }

    public async ValueTask<HomeRecommendInfo?> GetRecommend(RecommendModel recommendModel)
    {
        const string url = "/x/v2/feed/index";

        var paramList = new List<KeyValuePair<string, string>>()
        {
            new("idx", (recommendModel.Idx.ToString())),
            new("flush", (recommendModel.Flush ?? "0")),
            new("device", (recommendModel.Device ?? "0")),
            new("device_name", (recommendModel.DeviceName ?? "0")),
            new("column", (recommendModel.Column ?? "0")),
            new("pull", (recommendModel.Pull ?? "0")),
            new("mobi_app", "iphone"),
            new("platform", "ios"),
        };

        await SignParam(paramList, ApiPlatform.Ios);
        var request = await _appHttpClient.BuildRequestMessage(url, HttpMethod.Get, paramList);

        return await _appHttpClient.SendAsync<HomeRecommendInfo>(request);
    }

    public async ValueTask<object?> RegionIndex()
    {
        const string url = "/x/v2/region/index";

        var query = await SignParamQueryString();

        var request = await _appHttpClient.BuildRequestMessage(url + $"?{query}", HttpMethod.Get);
        return await _appHttpClient.SendAsync<object>(request);
    }

    public async ValueTask<object?> GetMyInfo(string accessToken)
    {
        await Task.CompletedTask;

        const string url = "/x/v2/account/myinfo";

        var paramList = new List<KeyValuePair<string, string>>()
        {
            new("access_key", accessToken),
        };

        var query = await SignParamQueryString(paramList);

        var request = await _appHttpClient.BuildRequestMessage(url + $"?{query}", HttpMethod.Get);
        return await _appHttpClient.SendAsync<object>(request);
    }
}