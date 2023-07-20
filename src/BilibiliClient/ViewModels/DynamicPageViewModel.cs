using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BilibiliClient.Core.Contracts.Models;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class DynamicPageViewModel : AbsPageViewModel
{
    public override NavBarType NavBarType => NavBarType.Dynamic;

    public bool IsVideo
    {
        get => _currentDataType == DynamicDataType.Video;
        set
        {
            CurrentDataType = value ? DynamicDataType.Video : DynamicDataType.All;
            RaisePropertyChanged();
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
    public ObservableCollection<object> DynamicDataList { get; } = new ObservableCollection<object>();

    public ICommand LoadMoreCmd =>
        _loadMoreCmd ??= new AsyncRelayCommand(LoadMoreData, () => _dynamicService.HasMore);

    private ICommand? _loadMoreCmd;
    private readonly IDynamicService _dynamicService;

    public DynamicPageViewModel(IDynamicService dynamicService)
    {
        _dynamicService = dynamicService;
    }

    public override async Task OnNavigatedTo(object? parameter = null)
    {
        await base.OnNavigatedTo(parameter);
        _dynamicService.ResetParam();
        DynamicDataList.Clear();
    }

    private async Task LoadMoreData()
    {
        var dataList = await _dynamicService.LoadNextPage(CurrentDataType);
        dataList.ForEach(DynamicDataList.Add);
    }
}