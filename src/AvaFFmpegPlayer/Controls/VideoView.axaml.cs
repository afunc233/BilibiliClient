using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;

namespace AvaFFmpegPlayer.Controls;

[TemplatePart(PART_Image, typeof(Image))]
public class VideoView : TemplatedControl, IVideoView
{
    private const string PART_Image = nameof(PART_Image);

    private Image _image;


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _image = e.NameScope.Find<Image>(PART_Image);
    }

    void IVideoView.UpdateBitmapSource(WriteableBitmap targetBitmap)
    {
        if (_image != null)
        {
            _image.Source = targetBitmap;
        }
    }

    void IVideoView.InvalidateVisual()
    {
        this._image.InvalidateVisual();
    }
}