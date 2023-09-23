using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;
using BilibiliClient.Core.Contracts.Services;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace BilibiliClient.Services;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<Type, ContentDialog> _typeDialogDic = new();

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    async Task<TT?> IDialogService.ShowDialog<T, TT>(object? parameter) where TT : default
    {
        return await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var viewModelT = _serviceProvider.GetRequiredService<T>() as IDialog<TT>;
            var cd = new ContentDialog
            {
                Content = viewModelT,
                Title = viewModelT.Title,
                CloseButtonText = viewModelT.CloseButtonText,
                IsPrimaryButtonEnabled = true,
                IsSecondaryButtonEnabled = true,
                DefaultButton = ContentDialogButton.None,
                FullSizeDesired = false
            };
            if (!string.IsNullOrWhiteSpace(viewModelT.PrimaryButtonText))
            {
                cd.PrimaryButtonText = viewModelT.PrimaryButtonText;
            }

            if (!string.IsNullOrWhiteSpace(viewModelT.SecondaryButtonText))
            {
                cd.SecondaryButtonText = viewModelT.SecondaryButtonText;
            }

            viewModelT.OnClose = () => Dispatcher.UIThread.Invoke(() => cd.Hide());
            _typeDialogDic.TryAdd(typeof(T), cd);
            await viewModelT.Init(parameter);

            await cd.ShowAsync();
            _typeDialogDic.TryRemove(typeof(T), out _);
            return viewModelT.Result;
        });
    }

    void IDialogService.CloseDialog<T>()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (_typeDialogDic.TryGetValue(typeof(T), out var cd))
            {
                cd?.Hide();
            }
        });
    }
}