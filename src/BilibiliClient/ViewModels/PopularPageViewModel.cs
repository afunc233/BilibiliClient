using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bilibili.App.Card.V1;
using Bilibili.App.Show.V1;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class PopularPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Popular;

    public ObservableCollection<Card> PopularCardList { get; } = new ObservableCollection<Card>();

    public ICommand LoadMoreCmd => _loadMoreCmd ??= new AsyncRelayCommand(DoLoadMore, () => CanLoadMore);
    private ICommand? _loadMoreCmd;

    private bool CanLoadMore = true;

    private long _idx;
    private readonly IGrpcApi _grpcApi;

    public PopularPageViewModel(IGrpcApi grpcApi,HeaderViewModel headerViewModel)
    {
        _grpcApi = grpcApi;
        Header = headerViewModel;
    }


    private async Task DoLoadMore()
    {
        IsLoading = true;
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
                PopularCardList.Add(popularReplyItem);
            }
            _idx = popularReply.Items.Last().SmallCoverV5.Base.Idx;
        }
        else
        {
            CanLoadMore = false;
        }

        IsLoading = false;
    }
}