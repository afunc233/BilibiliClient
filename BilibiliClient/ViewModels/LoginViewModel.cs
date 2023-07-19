using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using BilibiliClient.Core.Contracts.Api;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.ViewModels;

public class LoginViewModel : ViewModelBase
{
    public Action<bool>? OnClose { get; set; }

    public ICommand OnLoadCmd => _onLoadCmd ??= new RelayCommand(RefreshCode);
    private ICommand? _onLoadCmd;

    public ICommand OnUnloadCmd => _onUnloadCmd ??= new RelayCommand(() =>
    {
        _messenger.RegisterAll(this);
        CancelQRCodeLogin();
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
    private readonly ILoginService _loginService;

    private readonly IMessenger _messenger;

    public LoginViewModel(IMessenger messenger, ILoginService loginService)
    {
        _messenger = messenger;
        _loginService = loginService;
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
            case LoginStateEnum.Success:
                OnClose?.Invoke(true);
                break;
        }
    }

    private void CancelQRCodeLogin()
    {
        _qrCodeLoginTaskCompletionSource?.Cancel();
        _qrCodeLoginTaskCompletionSource = null;
    }

    public void RefreshCode()
    {
        CancelQRCodeLogin();
        _qrCodeLoginTaskCompletionSource = new CancellationTokenSource();

        Task.Run(async () =>
        {
            var loginQRCode = await _loginService.GetLoginQRCode();
            if (!string.IsNullOrWhiteSpace(loginQRCode))
            {
                QRCodeSource = loginQRCode;
                while (true)
                {
                    if (_qrCodeLoginTaskCompletionSource.IsCancellationRequested)
                    {
                        break;
                    }

                    await _loginService.CheckHasLogin();
                    await Task.Delay(3000);
                }
            }
            else
            {
                CancelQRCodeLogin();
            }
        }, _qrCodeLoginTaskCompletionSource.Token);
    }
}