using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Messages;

public class PlayVideoMessage<T>(T value) : ValueChangedMessage<T>(value)
{
}