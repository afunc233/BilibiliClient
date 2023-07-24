using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Core.Messages;

public class OpenViewMessage : AsyncRequestMessage<bool>
{
    public ViewType ViewType { get; }
    public object? Parameter { get; }

    public OpenViewMessage(ViewType viewType, object? parameter = null)
    {
        ViewType = viewType;
        Parameter = parameter;
    }
}

public enum ViewType
{
    Main,
    Login,
    Player,
}