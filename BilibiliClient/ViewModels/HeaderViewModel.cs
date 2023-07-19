using System.Windows.Input;
using BilibiliClient.Models.Messaging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.ViewModels;

public class HeaderViewModel : ViewModelBase
{
    public ICommand LoginCmd => _loginCmd ??= new AsyncRelayCommand(async () =>
    {
        var isSuccess = await _messenger.Send<StartLoginMessage>();
        if (isSuccess)
        {
            
        }
    });

    private ICommand? _loginCmd;


    private readonly IMessenger _messenger;

    public HeaderViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }
}