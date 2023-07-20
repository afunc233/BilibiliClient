using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BilibiliClient.Views;

public partial class PopularPageView : UserControl
{
    public PopularPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}