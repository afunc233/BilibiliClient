using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public unsafe class BufferReference(byte* target, long length) : NativeReference<byte>(target)
{
    public long Length { get; set; } = length;
}
