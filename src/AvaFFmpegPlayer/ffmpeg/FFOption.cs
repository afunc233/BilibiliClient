using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFOption(AVOption* target) : NativeReference<AVOption>(target)
{
    public AVOptionType Type => Target->type;
}
