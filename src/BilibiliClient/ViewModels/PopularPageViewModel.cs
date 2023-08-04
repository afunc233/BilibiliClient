using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bilibili.App.Card.V1;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;

namespace BilibiliClient.ViewModels;

public partial class PopularPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Popular;

    public ObservableCollection<Card> PopularCardList { get; } = new();


    private readonly IPopularService _popularService;

    public PopularPageViewModel(IPopularService popularService, HeaderViewModel headerViewModel)
    {
        _popularService = popularService;
        Header = headerViewModel;
    }

    protected override async Task LoadMore()
    {
        IsLoading = true;

        var popularCards = _popularService.Popular();
        var hasData = false;
        await foreach (Card popularCard in popularCards)
        {
            hasData = true;
            PopularCardList.Add(popularCard);
        }

        CanLoadMore = hasData;
        IsLoading = false;
    }
}