using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BilibiliClient.Views;

public partial class SettingPageView : UserControl
{
    public SettingPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}