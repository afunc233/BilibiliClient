using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Core.Messages;

public class OpenViewMessage(ViewType viewType, object? parameter = null) : AsyncRequestMessage<bool>
{
    public ViewType ViewType { get; } = viewType;
    public object? Parameter { get; } = parameter;
}

public enum ViewType
{
    Main,
    Login,
    Player,
}