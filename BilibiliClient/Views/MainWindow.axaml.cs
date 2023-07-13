using Avalonia.Controls;
using BilibiliClient.ViewModels;

namespace BilibiliClient.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel mainViewModel)
    {
        InitializeComponent();
        this.DataContext = mainViewModel;
    }
}