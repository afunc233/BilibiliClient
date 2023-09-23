using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using BilibiliClient.Messages;
using CommunityToolkit.Mvvm.Messaging;
using FluentAvalonia.UI.Windowing;
using DynamicData;

namespace BilibiliClient.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        SplashScreen = new MainAppSplashScreen(this)
        {
            InitApp = async () => await Task.CompletedTask,
        };
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        var messenger = this.GetAppRequiredService<IMessenger>();
        messenger.Register<GlobalIconMessage>(this, HandleGlobalIconMessage);
        this.Closed += (sender, args) => { messenger.UnregisterAll(this); };
    }

    private void HandleGlobalIconMessage(object recipient, GlobalIconMessage message)
    {
        Dispatcher.UIThread.Invoke(() => Icon = message.Value);
    }
}

internal class MainAppSplashScreen : IApplicationSplashScreen
{
    public MainAppSplashScreen(MainWindow owner)
    {
        _owner = owner;
    }

    public string AppName => "Bilibili";
    public IImage? AppIcon => null;
    public object SplashScreenContent => new MainAppSplashContent();
    public int MinimumShowTime => 1000;

    public Func<Task>? InitApp { get; init; }

    public async Task RunTasks(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (InitApp != null)
        {
            await InitApp.Invoke();
        }
    }

    // ReSharper disable once NotAccessedField.Local
    private MainWindow _owner;
}