using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.ViewModels;
using BilibiliClient.Views;
using Microsoft.Extensions.Hosting;

namespace BilibiliClient.Services;

public class BilibiliHostedService : IHostedService
{
    private readonly IWindowManagerService _windowManagerService;

    private readonly Avalonia.Controls.ApplicationLifetimes.IApplicationLifetime _applicationLifetime;

    public BilibiliHostedService(IWindowManagerService windowManagerService,
        Avalonia.Controls.ApplicationLifetimes.IApplicationLifetime applicationLifetime)
    {
        _windowManagerService = windowManagerService;
        _applicationLifetime = applicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
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

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}