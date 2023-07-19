using System;
using System.Threading.Tasks;
using BilibiliClient.Core.Contracts;
using BilibiliClient.Core.Contracts.Services;
using BilibiliClient.Core.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.Services.Handler;

public class UserSecretStartStopHandler : IStartStopHandler
{
    public int Order { get; } = 0;

    private readonly IMessenger _messenger;
    private readonly IUserSecretService _userSecretService;

    public UserSecretStartStopHandler(IMessenger messenger, IUserSecretService userSecretService)
    {
        _messenger = messenger;
        _userSecretService = userSecretService;
        _messenger = messenger;
    }

    private async void SaveUserSecretMessageHandler(object recipient, SaveUserSecretMessage message)
    {
        try
        {
            await _userSecretService.SaveUserSecret(message.Value);
        }
        catch (Exception e)
        {
            // Console.WriteLine(e);
        }
    }

    public async Task HandleStartAsync()
    {
        _messenger.Register<SaveUserSecretMessage>(this, SaveUserSecretMessageHandler);
        await Task.CompletedTask;
        await _userSecretService.LoadUserSecret();
    }

    public async Task HandleStopAsync()
    {
        _messenger.RegisterAll(this);
        await Task.CompletedTask;
        await _userSecretService.SaveUserSecret();
    }
}