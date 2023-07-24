﻿using System.Threading.Tasks;
using BilibiliClient.Core.Messages;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace BilibiliClient.ViewModels;

public partial class HeaderViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;

    public HeaderViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    public async Task Login()
    {
        var isSuccess = await _messenger.Send<StartLoginMessage>();
        if (isSuccess)
        {
        }
    }
}