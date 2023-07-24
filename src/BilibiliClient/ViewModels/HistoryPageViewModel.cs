using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bilibili.App.Interfaces.V1;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace BilibiliClient.ViewModels;

public partial class HistoryPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.History;

    public ObservableCollection<CursorItem> HistoryDataList { get; } = new ObservableCollection<CursorItem>();

    public ICommand LoadMoreCmd =>
        _loadMoreCmd ??= new AsyncRelayCommand(LoadMoreData, () => _historyService.HasMore);

    private ICommand? _loadMoreCmd;

    [ObservableProperty] private bool hasMore;

    private readonly IHistoryService _historyService;

    public HistoryPageViewModel(IHistoryService historyService)
    {
        _historyService = historyService;

        this.WhenAnyValue(it => it.HasMore, it => it.Header)
            .CombineLatest(
                Observable.FromEventPattern<DataErrorsChangedEventArgs>(x => ErrorsChanged += x,
                    x => ErrorsChanged -= x));
    }

    private event EventHandler<DataErrorsChangedEventArgs> onErrorsChanged;

    private event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
    {
        add => onErrorsChanged += value;
        remove => onErrorsChanged -= value;
    }

    [RelayCommand]
    private async Task LoadMoreData()
    {
        IsLoading = true;
        var list = await _historyService.LoadNextPage();
        if (list?.Any() ?? false)
        {
            list.ForEach(HistoryDataList.Add);
        }

        IsLoading = false;
    }

    public override async Task OnNavigatedTo(object? parameter = null)
    {
        HistoryDataList.Clear();
        await base.OnNavigatedTo(parameter);
        _historyService.ResetCursor();
    }
}