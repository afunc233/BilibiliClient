﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BilibiliClient.Core.Api.Models;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;

namespace BilibiliClient.ViewModels;

public class DynamicPageViewModel(IDynamicService dynamicService) : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Dynamic;

    public override string Title => "动态";

    public bool IsVideo
    {
        get => _currentDataType == DynamicDataType.Video;
        set
        {
            CurrentDataType = value ? DynamicDataType.Video : DynamicDataType.All;
            OnPropertyChanged();
        }
    }

    public DynamicDataType CurrentDataType
    {
        get => _currentDataType;

        set
        {
            if (SetProperty(ref _currentDataType, value))
            {
                DynamicDataList.Clear();
            }
        }
    }

    private DynamicDataType _currentDataType = DynamicDataType.Video;
    public ObservableCollection<object> DynamicDataList { get; } = new();

    private readonly IDynamicService _dynamicService = dynamicService;

    public override async Task OnNavigatedTo(object? parameter = null)
    {
        CanLoadMore = true;
        await base.OnNavigatedTo(parameter);
        _dynamicService.ResetParam();
        DynamicDataList.Clear();
    }

    protected override async Task LoadMore()
    {
        var dataList = await _dynamicService.LoadNextPage(CurrentDataType);
        dataList.ForEach(DynamicDataList.Add);
        CanLoadMore = _dynamicService.HasMore;
    }
}