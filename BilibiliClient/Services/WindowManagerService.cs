using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.ViewModels;
using BilibiliClient.Views;
using Microsoft.Extensions.DependencyInjection;

namespace BilibiliClient.Services;

internal class WindowManagerService : IWindowManagerService
{
    private readonly IClassicDesktopStyleApplicationLifetime _classicDesktopStyleApplicationLifetime;

    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<string, Type?> _pages = new Dictionary<string, Type?>();

    public WindowManagerService(IServiceProvider serviceProvider,
        IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
    {
        _serviceProvider = serviceProvider;
        _classicDesktopStyleApplicationLifetime = classicDesktopStyleApplicationLifetime;
        Configure<MainViewModel, MainWindow>();
    }

    private void Configure<TVm, TV>()
        where TVm : INotifyPropertyChanged
        where TV : Window
    {
        lock (_pages)
        {
            var key = typeof(TVm).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(TV);
            if (_pages.Any(p => p.Value == type))
            {
                throw new ArgumentException(
                    $"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }

    public Type? GetPageType(string key)
    {
        lock (_pages)
        {
            if (!_pages.TryGetValue(key, out var pageType))
            {
                throw new ArgumentException(
                    $"Page not found: {key}. Did you forget to call PageService.Configure?");
            }

            return pageType;
        }
    }

    void IWindowManagerService.CloseWindow(string key)
    {
        var window = GetWindow(key);
        window?.Close();
    }

    bool? IWindowManagerService.OpenInDialog(string pageKey, object? parameter)
    {
        return false;
    }

    void IWindowManagerService.OpenInNewWindow(string pageKey, object? parameter)
    {
        var window = GetWindow(pageKey);
        if (window != null)
        {
            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }

            window.Activate();
        }
        else
        {
            window = _serviceProvider.GetRequiredService(GetPageType(pageKey)!) as Window;
            window!.DataContext = _serviceProvider.GetRequiredService(Type.GetType(pageKey)!);
            window.Show();
        }

        if (window is not { DataContext: INavigationAware navigationAware }) return;
        window.Closed += (s, e) => { navigationAware.OnNavigatedFrom(); };
        navigationAware.OnNavigatedTo(parameter);
    }

    private Window? GetWindow(string pageKey)
    {
        return _classicDesktopStyleApplicationLifetime.Windows.FirstOrDefault(it =>
        {
            if (it.DataContext != null)
            {
                if (string.Equals(pageKey, it.DataContext.GetType().FullName))
                {
                    return true;
                }
            }

            return false;
        });
    }

    void IWindowManagerService.OpenInShallWindow(string key, object? parameter)
    {
    }
}