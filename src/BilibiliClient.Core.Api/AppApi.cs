using BilibiliClient.Core.Api.Configs;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Api.Contracts.ApiHttpClient;
using BilibiliClient.Core.Api.Contracts.Configs;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Api;

internal class AppApi : AbsApi, IAppApi
{
    private readonly IAppHttpClient _appHttpClient;

    public AppApi(IAppHttpClient appHttpClient, IEnumerable<IPlatformConfig> platformConfigs) : base(platformConfigs)
    {
        _appHttpClient = appHttpClient;
    }

    public async ValueTask<object?> SearchSquare(int from, int limit)
    {
        var url = "/x/v2/search/square";

        var paramList = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("device", "phone"),
            new KeyValuePair<string, string>("from", "0"),
            new KeyValuePair<string, string>("limit", "50"),
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
            new KeyValuePair<string, string>("idx", (recommendModel.Idx.ToString())),
            new KeyValuePair<string, string>("flush", (recommendModel.Flush ?? "0")),
            new KeyValuePair<string, string>("device", (recommendModel.Device ?? "0")),
            new KeyValuePair<string, string>("device_name", (recommendModel.DeviceName ?? "0")),
            new KeyValuePair<string, string>("column", (recommendModel.Column ?? "0")),
            new KeyValuePair<string, string>("pull", (recommendModel.Pull ?? "0")),
            new KeyValuePair<string, string>("mobi_app", "iphone"),
            new KeyValuePair<string, string>("platform", "ios"),
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
            new KeyValuePair<string, string>("access_key", accessToken),
        };

        var query = await SignParamQueryString(paramList);

        var request = await _appHttpClient.BuildRequestMessage(url + $"?{query}", HttpMethod.Get);
        return await _appHttpClient.SendAsync<object>(request);
    }
}