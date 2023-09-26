using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.Api;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.Core.Services;

internal class PlayerService : IPlayerService
{
    private readonly IGrpcApi _grpcApi;
    private readonly IApiApi _apiApi;

    public PlayerService(IGrpcApi grpcApi, IApiApi apiApi)
    {
        _grpcApi = grpcApi;
        _apiApi = apiApi;
    }

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
}