using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFSubtitleRect(AVSubtitleRect* target) : NativeReference<AVSubtitleRect>(target)
{
    public int X => Target->x;

    public int Y => Target->y;

    public int W => Target->w;

    public int H => Target->h;

    public byte_ptrArray4 Data => Target->data;

    public int_array4 LineSize => Target->linesize;
}
