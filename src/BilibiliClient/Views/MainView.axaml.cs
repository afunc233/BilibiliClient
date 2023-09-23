using Avalonia.Controls;
using Avalonia.Threading;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using BilibiliClient.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.Views;

public partial class MainView : UserControl
{
    private readonly IDialogService _dialogService;

    public MainView()
    {
        InitializeComponent();
        _dialogService = this.GetAppRequiredService<IDialogService>();
        var messenger = this.GetAppRequiredService<IMessenger>();

        this.AttachedToVisualTree += (_, _) =>
        {
            if (!messenger.IsRegistered<StartLoginMessage>(this))
            {
                messenger.Register<StartLoginMessage>(this, StartLoginMessageHandler);
            }
        };
        this.DetachedFromVisualTree += (_, _) => { messenger.UnregisterAll(this); };
    }

    private void StartLoginMessageHandler(object recipient, StartLoginMessage message)
    {
        message.Reply(Dispatcher.UIThread.InvokeAsync(async () => await _dialogService.ShowDialog<LoginViewModel, bool>()));
    }
}