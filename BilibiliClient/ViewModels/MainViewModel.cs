using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BilibiliClient.Core.Configs;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class MainViewModel : ViewModelBase
{
    public IPageViewModel? CurrentPage
    {
        get => _currentPage;
        set
        {
            SetProperty(ref _currentPage, value);
            RaisePropertyChanged(nameof(Header));
        }
    }

    private IPageViewModel? _currentPage;

    public ViewModelBase Header
    {
        get => CurrentPage?.Header ?? _header;
    }

    private readonly ViewModelBase _header;

    public ObservableCollection<NavBar> NavBarList { get; } = new ObservableCollection<NavBar>()
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


    public NavBar CurrentNavBar
    {
        get => _currentNavBar;
        set => SetProperty(ref _currentNavBar, value);
    }

    private NavBar _currentNavBar;


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

    private ICommand? _navBarChangedCmd;

    public ICommand DoSomeThingCmd =>
        _doSomeThingCmd ??= new AsyncRelayCommand(async () =>
        {
            var appApi = this.GetAppRequiredService<IAppApi>();
            // await appApi.RegionIndex();
            //
            // await appApi.SearchSquare();

            var accountService = this.GetAppRequiredService<IAccountService>();

            var aa = await accountService.IsLocalTokenValid(true);

            if (aa)
            {
            }

            // var bb = await accountService.RefreshToken();
            //
            // if (bb)
            // {
            // }

            await Task.CompletedTask;
        });

    private ICommand? _doSomeThingCmd;

    private readonly IEnumerable<IPageViewModel> _pageViewModels;

    public MainViewModel(IEnumerable<IPageViewModel> pageViewModels, HeaderViewModel headerViewModel)
    {
        _pageViewModels = pageViewModels;
        _header = headerViewModel;
        _currentNavBar = NavBarList.First();
    }
}