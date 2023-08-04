using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;
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
    }
}