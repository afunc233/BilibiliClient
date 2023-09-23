using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Messages;

public class GlobalIconMessage : ValueChangedMessage<string>
{
    public GlobalIconMessage(string value) : base(value)
    {
    }
}