using System.Threading.Tasks;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public  abstract partial class AbsPageViewModel : ViewModelBase, IPageViewModel
{
    public abstract NavBarType NavBarType { get; }

    public ViewModelBase? Header { get; protected init; }
    
    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _canLoadMore = true;
    
    public virtual async Task OnNavigatedTo(object? parameter = null)
    {
        IsLoading = false;
        await Task.CompletedTask;
    }

    public virtual async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanLoadMore))]
    protected virtual async Task LoadMore()
    {
        await Task.CompletedTask;
    }
}