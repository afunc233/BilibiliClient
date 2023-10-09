using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bilibili.App.Card.V1;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Messages;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.ViewModels;

public partial class PopularPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Popular;

    public override string Title => "热门";

    public ObservableCollection<Card> PopularCardList { get; } = new();


    private readonly IPopularService _popularService;

    private readonly IMessenger _messenger;

    public PopularPageViewModel(IPopularService popularService, HeaderViewModel headerViewModel, IMessenger messenger)
    {
        _popularService = popularService;
        Header = headerViewModel;
        _messenger = messenger;
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

    [RelayCommand]
    private async Task PlayVideo(Card? card)
    {
        _messenger.Send(new PlayVideoMessage<Card?>(card));
        await Task.CompletedTask;
    }
}