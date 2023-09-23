using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BilibiliClient.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.Views;

public partial class HeaderView : UserControl
{
    public HeaderView()
    {
        InitializeComponent();

        if (OperatingSystem.IsWindows())
        {
            void HandleGlobalIconMessage(object recipient, GlobalIconMessage message)
            {
                Dispatcher.UIThread.Invoke(() => Icon.Source = message.Value);
            }

            var messenger = this.GetAppRequiredService<IMessenger>();
            messenger.Register<GlobalIconMessage>(this, HandleGlobalIconMessage);
            this.Unloaded += (sender, args) => { messenger.UnregisterAll(this); };
        }
    }
}