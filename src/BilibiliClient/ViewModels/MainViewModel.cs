﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Card.V1;
using BilibiliClient.Core.Models.Https.App;
using BilibiliClient.Messages;
using BilibiliClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

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

    public ObservableCollection<NavBar> NavBarList { get; } = new()
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

    private IServiceProvider _serviceProvider;
    private readonly IEnumerable<IPageViewModel> _pageViewModels;
    private readonly IMessenger _messenger;

    public MainViewModel(IServiceProvider serviceProvider, IEnumerable<IPageViewModel> pageViewModels,
        HeaderViewModel headerViewModel,
        IMessenger messenger)
    {
        _serviceProvider = serviceProvider;
        _pageViewModels = pageViewModels;
        _header = headerViewModel;
        _messenger = messenger;
        _currentNavBar = NavBarList.First();

        _messenger.Register<PlayVideoMessage<RecommendCardItem?>>(this, HandlePlayVideoMessage);
        _messenger.Register<PlayVideoMessage<Card?>>(this, HandlePlayVideoMessage);
    }

    private async void HandlePlayVideoMessage(object recipient, PlayVideoMessage<Card?> message)
    {
        if (CurrentPage != null)
        {
            await CurrentPage.OnNavigatedFrom();
        }

        CurrentPage = _serviceProvider.GetRequiredService<PlayerPageViewModel>();
        await CurrentPage.OnNavigatedTo(message.Value);
    }

    private async void HandlePlayVideoMessage(object recipient, PlayVideoMessage<RecommendCardItem?> message)
    {
        if (CurrentPage != null)
        {
            await CurrentPage.OnNavigatedFrom();
        }


        // TODO CurrentPage = 
        CurrentPage = _serviceProvider.GetRequiredService<PlayerPageViewModel>();
        await CurrentPage.OnNavigatedTo(message.Value);
    }

    [RelayCommand]
    private async Task NavBarChanged(NavBar? navBar)
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
    }


    [RelayCommand]
    private async Task DoSomeThing()
    {
        await Task.CompletedTask;
    }
}