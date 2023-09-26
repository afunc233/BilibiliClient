using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFCodecParameters : NativeReference<AVCodecParameters>
{
    public FFCodecParameters(AVCodecParameters* target)
        : base(target)
    {
        // placeholder
    }

    public AVMediaType CodecType => Target->codec_type;

    public AVCodecID CodecId => Target->codec_id;

    public int SampleRate => Target->sample_rate;

    public int Channels => Target->channels;

    public int Width => Target->width;

    public int Height => Target->height;

    public AVRational SampleAspectRatio => Target->sample_aspect_ratio;
}
