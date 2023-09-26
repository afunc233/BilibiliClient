using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Messages;

public class PlayVideoMessage<T> : ValueChangedMessage<T>
{
    public PlayVideoMessage(T value) : base(value)
    {
    }
}