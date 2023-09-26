﻿using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFFilter : NativeReference<AVFilter>
{
    private FFFilter(AVFilter* pointer)
        : base(pointer)
    {
        // placeholder
    }

    public static FFFilter FromName(string name)
    {
        var pointer = ffmpeg.avfilter_get_by_name(name);
        return pointer is not null ? new FFFilter(pointer) : default;
    }
}
