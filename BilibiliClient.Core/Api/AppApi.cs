using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Core.Utils;

namespace BilibiliClient.Core.Api;

public class AppApi : IAppApi
{
    private readonly IAppHttpClient _appHttpClient;

    public AppApi(IAppHttpClient appHttpClient)
    {
        _appHttpClient = appHttpClient;
    }

    public async ValueTask<object?> SearchSquare()
    {
        var url = "x/v2/search/square";

        var request = await _appHttpClient.BuildRequestMessage(url, HttpMethod.Get);

        return await _appHttpClient.SendAsync<object>(request);
    }

    public async ValueTask<HomeRecommendInfo?> GetRecommend(RecommendModel recommendModel)
    {
        const string url = "x/v2/feed/index";

        var paramList = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("idx", (recommendModel.Idx ?? "0")),
            new KeyValuePair<string, string>("flush", (recommendModel.Flush ?? "0")),
            new KeyValuePair<string, string>("device", (recommendModel.Device ?? "0")),
            new KeyValuePair<string, string>("device_name", (recommendModel.DeviceName ?? "0")),
            new KeyValuePair<string, string>("column", (recommendModel.Column ?? "0")),
            new KeyValuePair<string, string>("pull", (recommendModel.Pull ?? "0")),
            new KeyValuePair<string, string>("mobi_app", "iphone"),
            new KeyValuePair<string, string>("platform", "ios"),
        };

        var appSigner = new AppSigner(AppSigner.AppSignerKeySec.IOSAppSignerKeySec);

        appSigner.appSign(paramList);
        var request = await _appHttpClient.BuildRequestMessage(url, HttpMethod.Get, paramList);

        return await _appHttpClient.SendAsync<HomeRecommendInfo>(request);
    }
}