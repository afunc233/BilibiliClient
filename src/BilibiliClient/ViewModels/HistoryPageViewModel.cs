﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Bilibili.App.Interfaces.V1;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public partial class HistoryPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.History;

    public ObservableCollection<CursorItem> HistoryDataList { get; } = new ObservableCollection<CursorItem>();

    public ICommand LoadMoreCmd =>
        _loadMoreCmd ??= new AsyncRelayCommand(LoadMoreData, () => _historyService.HasMore);

    private ICommand? _loadMoreCmd;


    private readonly IHistoryService _historyService;

    public HistoryPageViewModel(IHistoryService historyService)
    {
        _historyService = historyService;
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