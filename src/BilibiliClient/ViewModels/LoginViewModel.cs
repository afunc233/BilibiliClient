using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.ViewModels;

public class LoginViewModel : ViewModelBase, IDialog<bool>
{
    public bool Result { get; private set; }
    public Action? OnClose { get; set; }

    public string Title => "登录";

    public string CloseButtonText => "取消";

    public string? PrimaryButtonText { get; }

    public string? SecondaryButtonText { get; }

    public ICommand OnLoadCmd => _onLoadCmd ??= new RelayCommand(RefreshCode);
    private ICommand? _onLoadCmd;

    public ICommand OnUnloadCmd => _onUnloadCmd ??= new RelayCommand(() =>
    {
        CancelQRCodeLogin();
        _messenger.RegisterAll(this);
    });

    private ICommand? _onUnloadCmd;

    /// <summary>
    /// 二维码字符串
    /// </summary>
    public string? QRCodeSource
    {
        get => _qrCodeSource;
        set => SetProperty(ref _qrCodeSource, value);
    }

    private string? _qrCodeSource;

    private CancellationTokenSource? _qrCodeLoginTaskCompletionSource;
    private readonly IAccountService _accountService;

    private readonly IMessenger _messenger;

    public LoginViewModel(IMessenger messenger, IAccountService accountService)
    {
        _messenger = messenger;
        _accountService = accountService;
        _messenger.Register<LoginStateMessage>(this, LoginStateMessageHandler);
    }

    private void LoginStateMessageHandler(object recipient, LoginStateMessage message)
    {
        switch (message.Value)
        {
            case LoginStateEnum.StopQRCodePoll:
                CancelQRCodeLogin();
                break;
            case LoginStateEnum.Fail:
                CancelQRCodeLogin();
                break;
            case LoginStateEnum.QRCodeExpire:

                break;
            case LoginStateEnum.LoginSuccess:
                Result = true;
                OnClose?.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(LoginStateEnum));
        }
    }

    private void CancelQRCodeLogin()
    {
        _qrCodeLoginTaskCompletionSource?.Cancel();
        _qrCodeLoginTaskCompletionSource = null;
    }

    public async Task Init(object? parameter = null)
    {
        await Task.CompletedTask;
    }

    public void RefreshCode()
    {
        CancelQRCodeLogin();
        _qrCodeLoginTaskCompletionSource = new CancellationTokenSource();

        Task.Run(async () =>
        {
            var loginQRCode = await _accountService.GetLoginQRCode();
            if (!string.IsNullOrWhiteSpace(loginQRCode))
            {
                QRCodeSource = loginQRCode;
                while (true)
                {
                    await Task.Delay(3000);
                    if (_qrCodeLoginTaskCompletionSource.IsCancellationRequested)
                    {
                        break;
                    }

                    await _accountService.CheckLoginState();
                }
            }
            else
            {
                CancelQRCodeLogin();
            }
        }, _qrCodeLoginTaskCompletionSource.Token);
    }
}