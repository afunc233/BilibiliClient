using System.Threading.Tasks;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Models.Https.Api;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BilibiliClient.ViewModels;

public partial class PlayerViewModel : ViewModelBase, INavigationAware
{
    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty] private VideoPlayUrlResult? _videoPlayUrl;

    // private readonly IGrpcApi _grpcApi;
    // private readonly IApiApi _apiApi;

    // internal PlayerViewModel(IGrpcApi grpcApi, IApiApi apiApi)
    // {
    //     _grpcApi = grpcApi;
    //     _apiApi = apiApi;
    // }
    
    public async Task OnNavigatedTo(object? parameter = null)
    {
        // if (parameter is RecommendCardItem recommendCardItem)
        // {
        //     var view = await _grpcApi.GetVideoDetailByBVId(recommendCardItem.Bvid);
        //     if (view != null)
        //     {
        //         var videoPlayUrl = await _apiApi.GetVideoPlayUrl(view.Arc.Aid.ToString(),
        //             view.Pages.FirstOrDefault()?.Page?.Cid.ToString() ?? "");
        //         if (videoPlayUrl != null)
        //         {
        //             VideoPlayUrl = videoPlayUrl;
        //         }
        //     }
        // }
        
        await Task.CompletedTask;
    }

    public async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }
}