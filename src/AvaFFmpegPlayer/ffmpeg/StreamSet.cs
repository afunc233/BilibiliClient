using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class StreamSet(FFFormatContext parent) : NativeChildSet<FFFormatContext, FFStream>(parent)
{
    public override FFStream this[int index]
    {
        get => new(Parent.Target->streams[index], Parent);
        set => Parent.Target->streams[index] = value.Target;
    }

    public override int Count => Convert.ToInt32(Parent.Target->nb_streams);
}
