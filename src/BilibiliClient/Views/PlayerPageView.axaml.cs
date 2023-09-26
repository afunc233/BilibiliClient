using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BilibiliClient.ViewModels;

namespace BilibiliClient.Views;

public partial class PlayerPageView : UserControl
{
    public PlayerPageView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (this.DataContext is PlayerPageViewModel playerPageViewModel)
        {
            playerPageViewModel.SetVideoView(VideoView);
        }
    }
}