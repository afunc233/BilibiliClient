using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFOption : NativeReference<AVOption>
{
    public FFOption(AVOption* target)
        : base(target)
    {

    }

    public AVOptionType Type => Target->type;
}
