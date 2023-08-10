using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;

namespace BilibiliClient.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        SplashScreen = new MainAppSplashScreen(this);
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }
}

internal class MainAppSplashScreen : IApplicationSplashScreen
{
    public MainAppSplashScreen(MainWindow owner)
    {
        _owner = owner;
    }

    public string? AppName => "Bilibili";
    public IImage? AppIcon => null;
    public object SplashScreenContent => new MainAppSplashContent();
    public int MinimumShowTime => 1000;

    public Action? InitApp { get; set; }

    public Task RunTasks(CancellationToken cancellationToken)
    {
        if (InitApp == null)
            return Task.CompletedTask;

        return Task.Run(InitApp, cancellationToken);
    }

    // ReSharper disable once NotAccessedField.Local
    private MainWindow _owner;
}