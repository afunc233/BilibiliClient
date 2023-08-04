using Avalonia.Controls;
using Avalonia.Threading;
using BilibiliClient.Core.Messages;
using BilibiliClient.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using FluentAvalonia.UI.Controls;

namespace BilibiliClient.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
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
        message.Reply(Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var loginViewModel = this.GetAppRequiredService<LoginViewModel>();
            var cd = new ContentDialog
            {
                CloseButtonText = "取消",
                Title = "登录",
                Content = loginViewModel,
                IsPrimaryButtonEnabled = false,
                IsSecondaryButtonEnabled = false,
                DefaultButton = ContentDialogButton.None,
                FullSizeDesired = false
            };
            loginViewModel.OnClose = b =>
            {
                var aa = b ? ContentDialogResult.Primary : ContentDialogResult.None;
                Dispatcher.UIThread.Invoke(() => cd.Hide(aa));
            };
            var result = await cd.ShowAsync();
            return result != ContentDialogResult.None;
        }));
    }
}