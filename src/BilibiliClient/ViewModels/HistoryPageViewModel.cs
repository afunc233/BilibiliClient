﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Interfaces.V1;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;

namespace BilibiliClient.ViewModels;

public class HistoryPageViewModel(IHistoryService historyService) : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.History;

    public override string Title => "历史";

    public ObservableCollection<CursorItem> HistoryDataList { get; } = new();

    private readonly IHistoryService _historyService = historyService;

    protected override async Task LoadMore()
    {
        IsLoading = true;
        var list = await _historyService.LoadNextPage();
        if (list?.Any() ?? false)
        {
            list.ForEach(HistoryDataList.Add);
        }

        CanLoadMore = _historyService.HasMore;
        IsLoading = false;
    }

    public override async Task OnNavigatedTo(object? parameter = null)
    {
        CanLoadMore = true;
        HistoryDataList.Clear();
        await base.OnNavigatedTo(parameter);
        _historyService.ResetCursor();
    }
}