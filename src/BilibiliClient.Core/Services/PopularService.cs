using Bilibili.App.Card.V1;
using Bilibili.App.Show.V1;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;

namespace BilibiliClient.Core.Services;

internal class PopularService(IGrpcApi grpcApi) : IPopularService
{
    private long _idx = 0;
    private readonly IGrpcApi _grpcApi = grpcApi;

    public async IAsyncEnumerable<Card> Popular()
    {
        var isLogin = false;
        var popularReq = new PopularResultReq()
        {
            Idx = _idx,
            LoginEvent = isLogin ? 2 : 1,
            Qn = 112,
            Fnval = 464,
            Fourk = 1,
            Spmid = "creation.hot-tab.0.0",
            PlayerArgs = new Bilibili.App.Archive.Middleware.V1.PlayerArgs
            {
                Qn = 112,
                Fnval = 464,
            },
        };
        var popularReply = await _grpcApi.Popular(popularReq);
        if (popularReply is { Items: not null } && popularReply.Items.Any())
        {
            foreach (var popularReplyItem in popularReply.Items)
            {
                yield return popularReplyItem;
            }

            _idx = popularReply.Items.Last().SmallCoverV5.Base.Idx;
        }
    }
}