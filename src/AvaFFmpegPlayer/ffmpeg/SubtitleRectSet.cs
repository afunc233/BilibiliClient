using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class SubtitleRectSet(FFSubtitle parent) : NativeChildSet<FFSubtitle, FFSubtitleRect>(parent)
{
    public override FFSubtitleRect this[int index]
    {
        get => new(Parent.Target->rects[index]);
        set => Parent.Target->rects[index] = value.IsNotNull() ? value.Target : default;
    }

    public override int Count =>
        Convert.ToInt32(Parent.Target->num_rects);
}
