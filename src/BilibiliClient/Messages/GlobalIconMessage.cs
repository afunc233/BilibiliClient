using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Messages;

public class GlobalIconMessage : ValueChangedMessage<Bitmap>
{
    public GlobalIconMessage(Bitmap value) : base(value)
    {
    }
}