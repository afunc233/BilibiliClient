using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Messages;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.ViewModels;

public partial class RecommendPageViewModel(IRecommendService recommendService, IMessenger messenger) : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Recommend;

    public override string Title => "推荐";

    public ObservableCollection<RecommendCardItem> RecommendDataList { get; } = new();

    private readonly IRecommendService _recommendService = recommendService;
    private readonly IMessenger _messenger = messenger;

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
        _messenger.Send(new PlayVideoMessage<RecommendCardItem?>(recommendCardItem));
        await Task.CompletedTask;
    }
}