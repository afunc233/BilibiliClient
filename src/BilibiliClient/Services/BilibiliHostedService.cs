using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using BilibiliClient.ViewModels;
using BilibiliClient.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Hosting;

namespace BilibiliClient.Services;

public class BilibiliHostedService : IHostedService
{
    private readonly IWindowManagerService _windowManagerService;

    private readonly IMessenger _messenger;
    private readonly Avalonia.Controls.ApplicationLifetimes.IApplicationLifetime _applicationLifetime;

    public BilibiliHostedService(IWindowManagerService windowManagerService,
        IMessenger messenger,
        Avalonia.Controls.ApplicationLifetimes.IApplicationLifetime applicationLifetime)
    {
        _windowManagerService = windowManagerService;
        _messenger = messenger;
        _applicationLifetime = applicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _messenger.Register<OpenViewMessage>(this, OpenViewMessageHandler);
        switch (_applicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime:
                _windowManagerService.OpenInNewWindow(typeof(MainViewModel).FullName);
                break;
            case ISingleViewApplicationLifetime singleViewApplicationLifetime:
            {
                // Android / iOS
                var mainView = this.GetAppRequiredService<MainView>();
                mainView.DataContext = this.GetAppRequiredService<MainViewModel>();
                singleViewApplicationLifetime.MainView = mainView;
                break;
            }
        }

        await Task.CompletedTask;
    }

    private void OpenViewMessageHandler(object recipient, OpenViewMessage message)
    {
        switch (message.ViewType)
        {
            case ViewType.Main:
                _windowManagerService.OpenInNewWindow(typeof(MainViewModel).FullName);
                break;
            case ViewType.Player:
                break;
            case ViewType.Login:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}