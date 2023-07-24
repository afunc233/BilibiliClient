﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData.Binding;
using ReactiveUI;

namespace BilibiliClient.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(Header))]
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
            NavType = NavBarType.History,
            BarName = "历史",
            IconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/S5eFhs5ei0xBgoEIuUzenySo.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            CheckedIconUrl =
                "https://dev-s-image.vcinema.cn/new_navigation_icon/pMLVQw1DUC9Rr8lrpCJepWqA.jpg?x-oss-process=image/interlace,1/resize,m_fill,w_48,h_48/quality,q_100/sharpen,100/format,png",
            Foreground = "#ffffff",
            CheckedForeground = "#ff0000",
        },
        new NavBar()
        {
            NavType = NavBarType.Dynamic,
            BarName = "动态",
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

    [ObservableProperty] private NavBar _currentNavBar;


    public ICommand NavBarChangedCmd => _navBarChangedCmd ??= new AsyncRelayCommand<NavBar>(async (navBar) =>
    {
        if (navBar == null)
        {
            return;
        }

        var currentPage = _pageViewModels.FirstOrDefault(it => it.NavBarType == navBar.NavType);
        if (currentPage != null)
        {
            if (CurrentPage != null)
            {
                await CurrentPage.OnNavigatedFrom();
            }

            // 确保先 OnNavigatedTo 再设置到界面上
            await currentPage.OnNavigatedTo();
            CurrentPage = currentPage;
        }
    });

    private ICommand? _navBarChangedCmd;

    public ICommand DoSomeThingCmd =>
        _doSomeThingCmd ??= new AsyncRelayCommand(async () =>
        {
            var appApi = this.GetAppRequiredService<IAppApi>();

            var accountService = this.GetAppRequiredService<IAccountService>();

            var grpcApi = this.GetAppRequiredService<IGrpcApi>();
            var apiApi = this.GetAppRequiredService<IApiApi>();
            if (CurrentPage is RecommendPageViewModel recommendPageViewModel)
            {
                var data = recommendPageViewModel.RecommendDataList.FirstOrDefault();
                if (data != null)
                {
                    var view = await grpcApi.GetVideoDetailByBVId(data.Bvid);
                    if (view != null)
                    {
                        var aa = await apiApi.GetVideoPlayUrl(view.Arc.Aid.ToString(),
                            view.Pages.FirstOrDefault()?.Page?.Cid.ToString() ?? "");
                        if (aa != null)
                        {
                        }
                    }
                }
            }
        });

    private ICommand? _doSomeThingCmd;

    private readonly IEnumerable<IPageViewModel> _pageViewModels;

    public MainViewModel(IEnumerable<IPageViewModel> pageViewModels, HeaderViewModel headerViewModel)
    {
        _pageViewModels = pageViewModels;
        _header = headerViewModel;
        _currentNavBar = NavBarList.First();

        this.WhenValueChanged(it => it.CurrentPage).Subscribe(it => { Console.WriteLine($"{it?.NavBarType}"); });
        this.WhenAnyValue(it => it.CurrentPage).Subscribe(it => { Console.WriteLine($"{it?.NavBarType}"); });
    }
}