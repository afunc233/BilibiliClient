using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BilibiliClient.Core.Api.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;
using BilibiliClient.Views;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public partial class RecommendPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Recommend;
    public ObservableCollection<RecommendCardItem> RecommendDataList { get; } = new();

    private readonly IRecommendService _recommendService;

    public RecommendPageViewModel(IRecommendService recommendService)
    {
        _recommendService = recommendService;
    }

    protected override async Task LoadMore()
    {
        IsLoading = true;
        await Task.CompletedTask;


        var homeRecommends = _recommendService.GetRecommend();
        var hasData = false;
        await foreach (var homeRecommend in homeRecommends)
        {
            hasData = true;
            RecommendDataList.Add(homeRecommend);
        }

        CanLoadMore = hasData;
        IsLoading = false;
    }


    [RelayCommand]
    private async Task PlayVideo(RecommendCardItem? recommendCardItem)
    {
        await Task.CompletedTask;

        var vlcPlayerView = new VlcPlayerWindow();
        var playerViewModel = this.GetAppRequiredService<PlayerViewModel>();
        vlcPlayerView.DataContext = playerViewModel;
        vlcPlayerView.Show();
        await playerViewModel.OnNavigatedTo(recommendCardItem);
    }
}