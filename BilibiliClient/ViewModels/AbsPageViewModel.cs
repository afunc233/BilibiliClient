using System.Threading.Tasks;
using BilibiliClient.Models;

namespace BilibiliClient.ViewModels;

public abstract class AbsPageViewModel : ViewModelBase, IPageViewModel
{
    public abstract NavBarType NavBarType { get; }

    public ViewModelBase? Header { get; protected set; }

    public bool IsLoading
    {
        get => _isLoading;
        protected set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoading = false;

    public virtual async Task OnNavigatedTo(object? parameter = null)
    {
        IsLoading = false;
        await Task.CompletedTask;
    }

    public virtual async Task OnNavigatedFrom()
    {
        await Task.CompletedTask;
    }
}