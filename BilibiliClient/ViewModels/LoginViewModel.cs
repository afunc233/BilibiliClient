using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public Action<bool>? OnClose { get; set; }

    public ICommand OnCloseCmd => _onCloseCmd ??= new RelayCommand(() => OnClose?.Invoke(true));
    private ICommand? _onCloseCmd;
}