using BilibiliClient.Core.Contracts;
using BilibiliClient.Models;

namespace BilibiliClient.ViewModels;

public interface IPageViewModel : INavigationAware
{
    NavBarType NavBarType { get; }
    
    string Title { get; }
    
    ViewModelBase? Header { get; }

    bool IsLoading { get; }
}