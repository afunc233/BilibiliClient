using System.Linq;
using System.Threading.Tasks;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Models.Https.Api;
using BilibiliClient.Core.Models.Https.App;

namespace BilibiliClient.ViewModels;

public class PlayerViewModel : ViewModelBase, INavigationAware
{
    public VideoPlayUrlResult? VideoPlayUrl
    {
        get => _videoPlayUrl;
        set => SetProperty(ref _videoPlayUrl, value);
    }

    private VideoPlayUrlResult? _videoPlayUrl;

    private readonly IGrpcApi _grpcApi;
    private readonly IApiApi _apiApi;

    public PlayerViewModel(IGrpcApi grpcApi, IApiApi apiApi)
    {
        _grpcApi = grpcApi;
        _apiApi = apiApi;
    }

    public async Task OnNavigatedTo(object? parameter = null)
    {
        if (parameter is RecommendCardItem recommendCardItem)
        {
            var view = await _grpcApi.GetVideoDetailByBVId(recommendCardItem.Bvid);
            if (view != null)
            {
                var videoPlayUrl = await _apiApi.GetVideoPlayUrl(view.Arc.Aid.ToString(),
                    view.Pages.FirstOrDefault()?.Page?.Cid.ToString() ?? "");
                if (videoPlayUrl != null)
                {
                    VideoPlayUrl = videoPlayUrl;
                }
            }
        }

        await Task.CompletedTask;
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }
}