using System.Windows.Input;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Models.Https.App;
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

        var cc = await _appApi.GetRecommend(new RecommendModel());
        if (cc != null)
        {
        }
    });

    private ICommand? doSomeThingCmd;

    private readonly IAccountApi _accountApi;
    private readonly IAppApi _appApi;

    public MainViewModel(IAccountApi accountApi, IAppApi appApi)
    {
        _accountApi = accountApi;
        _appApi = appApi;
    }
}