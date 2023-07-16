using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BilibiliClient.Views;

public partial class RecommendPageView : UserControl
{
    public RecommendPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}