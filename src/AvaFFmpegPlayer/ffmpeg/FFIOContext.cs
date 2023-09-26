﻿using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFIOContext : NativeReference<AVIOContext>
{
    public FFIOContext(AVIOContext* target)
        : base(target)
    {
        // placeholder
    }

    public int Error => Target->error;

    public long BytePosition => ffmpeg.avio_tell(Target);

    public long Size => ffmpeg.avio_size(Target);

    public bool TestEndOfStream() => ffmpeg.avio_feof(Target) != 0;

    public bool EndOfStream
    {
        get => Address.IsNotNull() && Target->eof_reached != 0;
        set => Target->eof_reached = (value) ? 1 : 0;
    }
}
