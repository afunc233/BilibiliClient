using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFSubtitle : CountedReference<AVSubtitle>
{
    public FFSubtitle([CallerFilePath] string filePath = default, [CallerLineNumber] int? lineNumber = default)
        : base(filePath, lineNumber)
    {
        Update((AVSubtitle*)ffmpeg.av_mallocz((ulong)sizeof(AVSubtitle)));
    }

    public long Pts
    {
        get => Target->pts;
    }

    public long StartDisplayTime => Target->start_display_time;

    public long EndDisplayTime => Target->end_display_time;

    public int Format => Target->format;

    public SubtitleRectSet Rects => new(this);

    protected override unsafe void ReleaseInternal(AVSubtitle* pointer) =>
        ffmpeg.avsubtitle_free(pointer);
}
