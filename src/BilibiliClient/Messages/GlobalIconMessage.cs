using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace BilibiliClient.Messages;

public class GlobalIconMessage(Bitmap value) : ValueChangedMessage<Bitmap>(value)
{
}