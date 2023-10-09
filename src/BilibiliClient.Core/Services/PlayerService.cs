using Bilibili.App.Card.V1;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.Api;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Services;

internal class PlayerService(IGrpcApi grpcApi, IApiApi apiApi) : IPlayerService
{
    private readonly IGrpcApi _grpcApi = grpcApi;
    private readonly IApiApi _apiApi = apiApi;

    async Task<VideoPlayUrlResult?> IPlayerService.GetPlayUrl(RecommendCardItem recommendCardItem)
    {
        await Task.CompletedTask;

        var view = await _grpcApi.GetVideoDetailByBVId(recommendCardItem.Bvid);
        if (view != null)
        {
            var videoPlayUrl = await _apiApi.GetVideoPlayUrl(view.Arc.Aid.ToString(),
                view.Pages.FirstOrDefault()?.Page?.Cid.ToString() ?? "");
            if (videoPlayUrl != null)
            {
                return videoPlayUrl;
            }
        }

        return null;
    }

    async Task<VideoPlayUrlResult?> IPlayerService.GetPlayUrl(Card card)
    {
        await Task.CompletedTask;

        var bVid = string.Empty;

        switch (card.ItemCase)
        {
            case Card.ItemOneofCase.SmallCoverV5:
                bVid = card.SmallCoverV5.Base.ThreePointV4.WatchLater.Bvid;
                break;
        }

        if (!string.IsNullOrWhiteSpace(bVid))
        {
            var view = await _grpcApi.GetVideoDetailByBVId(bVid);
            if (view != null)
            {
                var videoPlayUrl = await _apiApi.GetVideoPlayUrl(view.Arc.Aid.ToString(),
                    view.Pages.FirstOrDefault()?.Page?.Cid.ToString() ?? "");
                if (videoPlayUrl != null)
                {
                    return videoPlayUrl;
                }
            }
        }

        return null;
    }
}