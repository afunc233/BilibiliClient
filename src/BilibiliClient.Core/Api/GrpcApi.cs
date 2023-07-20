using Bilibili.App.Interfaces.V1;
using Bilibili.App.Show.V1;
using BilibiliClient.Core.Configs;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;

namespace BilibiliClient.Core.Api;

public class GrpcApi : IGrpcApi
{
    private readonly IGrpcHttpClient _grpcHttpClient;
    private readonly UserSecretConfig _userSecretConfig;

    public GrpcApi(IGrpcHttpClient grpcHttpClient, UserSecretConfig userSecretConfig)
    {
        _grpcHttpClient = grpcHttpClient;
        _userSecretConfig = userSecretConfig;
    }

    public async ValueTask<PopularReply?> Popular(PopularResultReq popularReq)
    {
        await Task.CompletedTask;
        const string url = "/bilibili.app.show.v1.Popular/Index";

        var request = await _grpcHttpClient.BuildRequestMessage(url, popularReq);

        return await _grpcHttpClient.SendAsync(request, PopularReply.Parser);
    }

    public async ValueTask<RankListReply?> RankRegion(RankRegionResultReq popularReq)
    {
        await Task.CompletedTask;
        const string url = "bilibili.app.show.v1.Rank/RankRegion";

        var request = await _grpcHttpClient.BuildRequestMessage(url, popularReq);

        return await _grpcHttpClient.SendAsync(request, RankListReply.Parser);
    }

    public async ValueTask<CursorV2Reply?> GetMyHistory(Cursor historyCursor, string tabSign)
    {
        if (string.IsNullOrWhiteSpace(_userSecretConfig.AccessToken))
        {
            return default;
        }

        const string url = "bilibili.app.interface.v1.History/CursorV2";

        var req = new CursorV2Req
        {
            Business = tabSign,
            Cursor = historyCursor,
        };

        var request = await _grpcHttpClient.BuildRequestMessage(url, req, _userSecretConfig.AccessToken);

        return await _grpcHttpClient.SendAsync(request, CursorV2Reply.Parser);
    }
}