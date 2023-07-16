using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BilibiliClient.Core.Api;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class MainViewModel : ViewModelBase
{
    private IPageViewModel? _currentPage;

    public IPageViewModel? CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    private readonly ObservableCollection<NavBar> _navBarList = new ObservableCollection<NavBar>()
    {
        new NavBar()
        {
            NavType = NavBarType.Recommend,
            BarName = "推荐",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },
        new NavBar()
        {
            NavType = NavBarType.Popular,
            BarName = "热门",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },
        new NavBar()
        {
            NavType = NavBarType.Setting,
            BarName = "更多",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },
    };

    public ObservableCollection<NavBar> NavBarList => _navBarList;

    private NavBar _currentNavBar;

    public NavBar CurrentNavBar
    {
        get => _currentNavBar;
        set => SetProperty(ref _currentNavBar, value);
    }

    private ICommand? _navBarChangedCmd;

    public ICommand NavBarChangedCmd => _navBarChangedCmd ??= new AsyncRelayCommand<NavBar>(async (navBar) =>
    {
        if (navBar == null)
        {
            return;
        }

        if (CurrentPage != null)
        {
            await CurrentPage.OnNavigatedFrom();
        }

        CurrentPage = _pageViewModels.First(it => it.NavBarType == navBar.NavType);
        await CurrentPage.OnNavigatedTo();
    });

    public ICommand DoSomeThingCmd =>
        _doSomeThingCmd ??= new AsyncRelayCommand(async () => { await Task.CompletedTask; });

    private ICommand? _doSomeThingCmd;

    private readonly IEnumerable<IPageViewModel> _pageViewModels;

    public MainViewModel(IEnumerable<IPageViewModel> pageViewModels)
    {
        _pageViewModels = pageViewModels;
        _currentNavBar = NavBarList.First();
    }
}