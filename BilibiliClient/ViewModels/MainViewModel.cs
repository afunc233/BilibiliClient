using System.Windows.Input;
using BilibiliClient.Core.Contracts.Api;
using CommunityToolkit.Mvvm.Input;

namespace BilibiliClient.ViewModels;

public class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public ICommand DoSomeThingCmd => doSomeThingCmd ??= new AsyncRelayCommand(async () =>
    {
        var aa = await _accountApi.LoginCaptcha();
        if (aa != null)
        {
        }
        var bb = await _accountApi.CountryList();
        if (bb != null)
        {
        }
    });

    private ICommand? doSomeThingCmd;

    private readonly IAccountApi _accountApi;

    public MainViewModel(IAccountApi accountApi)
    {
        _accountApi = accountApi;
    }
}