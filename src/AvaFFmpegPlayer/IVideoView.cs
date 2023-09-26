using Avalonia.Media.Imaging;

namespace AvaFFmpegPlayer;

public interface IVideoView
{
    void UpdateBitmapSource(WriteableBitmap targetBitmap);

    void InvalidateVisual();
}