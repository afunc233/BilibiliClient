using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;
using BilibiliClient.Views;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class RecommendPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Recommend;
    public ObservableCollection<RecommendCardItem> RecommendDataList { get; } = new();

    public ICommand LoadMoreCmd => _loadMoreCmd ??=
        new AsyncRelayCommand(async () => await DoLoadMore());

    private ICommand? _loadMoreCmd;

    public ICommand PlayVideoCmd => _playVideoCmd ??= new AsyncRelayCommand<RecommendCardItem>(DoPlayVideo);
    private ICommand? _playVideoCmd;

    private long _idx = 0;
    private readonly IAppApi _appApi;

    public RecommendPageViewModel(IAppApi appApi)
    {
        _appApi = appApi;
    }

    private async Task DoLoadMore()
    {
        IsLoading = true;
        await Task.CompletedTask;
        var homeRecommend = await _appApi.GetRecommend(new RecommendModel()
        {
            Idx = _idx,
        });
        if (homeRecommend is { Items: not null })
        {
            homeRecommend.Items.ForEach(RecommendDataList.Add);
            var newIdx = homeRecommend.Items.LastOrDefault()?.Idx ?? 0;
            if (_idx < newIdx)
            {
                _idx = newIdx;
            }
        }

        IsLoading = false;
    }


    private async Task DoPlayVideo(RecommendCardItem? recommendCardItem)
    {
        await Task.CompletedTask;

        var vlcplayerView = new VlcPlayerWindow();
        var playerViewModel = this.GetAppRequiredService<PlayerViewModel>();
        vlcplayerView.DataContext = playerViewModel;
        vlcplayerView.Show();
        await playerViewModel.OnNavigatedTo(recommendCardItem);
    }
}