using Bilibili.App.Show.V1;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.ApiHttpClient;

namespace BilibiliClient.Core.Api;

public class GrpcApi : IGrpcApi
{
    private IGrpcHttpClient _grpcHttpClient;

    public GrpcApi(IGrpcHttpClient grpcHttpClient)
    {
        _grpcHttpClient = grpcHttpClient;
    }

    public async ValueTask<PopularReply?> Popular(PopularResultReq popularReq)
    {
        await Task.CompletedTask;
        const string url = "/bilibili.app.show.v1.Popular/Index";

        var request = await _grpcHttpClient.BuildRequestMessage(url, popularReq, false);

        return await _grpcHttpClient.SendAsync(request, PopularReply.Parser);
    }

    public async ValueTask<RankListReply?> RankRegion(RankRegionResultReq popularReq)
    {
        await Task.CompletedTask;
        const string url = "bilibili.app.show.v1.Rank/RankRegion";

        var request = await _grpcHttpClient.BuildRequestMessage(url, popularReq, false);

        return await _grpcHttpClient.SendAsync(request, RankListReply.Parser);
    }
}