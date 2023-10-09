using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFChapter(AVChapter* target) : NativeReference<AVChapter>(target)
{
    public long StartTime => Target->start;

    public AVRational TimeBase => Target->time_base;
}
