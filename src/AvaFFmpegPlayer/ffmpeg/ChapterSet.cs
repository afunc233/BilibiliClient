using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class ChapterSet(FFFormatContext parent) : NativeChildSet<FFFormatContext, FFChapter>(parent)
{
    public override FFChapter this[int index]
    {
        get => new(Parent.Target->chapters[index]);
        set => Parent.Target->chapters[index] = value.Target;
    }

    public override int Count => Convert.ToInt32(Parent.Target->nb_chapters);
}
