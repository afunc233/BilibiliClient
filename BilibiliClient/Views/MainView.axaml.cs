using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using BilibiliClient.Models.Messaging;
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

        this.AttachedToVisualTree += (sender, args) =>
        {
            if (!messenger.IsRegistered<StartLoginMessage>(this))
            {
                messenger.Register<StartLoginMessage>(this, Handler);
            }
        };
        this.DetachedFromVisualTree += (sender, args) => { messenger.UnregisterAll(this); };
    }

    private void Handler(object recipient, StartLoginMessage message)
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
                DefaultButton = ContentDialogButton.Close,
                FullSizeDesired = true
            };
            loginViewModel.OnClose = b => cd.Hide();
            var result = await cd.ShowAsync();
            return result == ContentDialogResult.None;
        }));
    }
}