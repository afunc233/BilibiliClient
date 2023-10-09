namespace BilibiliClient.Core.Contracts.Services;

public interface IDialog
{
    Task Init(object? parameter = null);

    Action? OnClose { protected get; set; }
}

public interface IDialog<out T> : IDialog
{
    string Title => "提示";

    string? CloseButtonText => "取消";

    string? PrimaryButtonText { get; }

    string? SecondaryButtonText { get; }

    T? Result { get; }
}