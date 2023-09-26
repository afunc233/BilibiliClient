using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFChapter : NativeReference<AVChapter>
{
    public FFChapter(AVChapter* target)
        : base(target)
    {
        // placeholder
    }

    public long StartTime => Target->start;

    public AVRational TimeBase => Target->time_base;
}
