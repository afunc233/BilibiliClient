﻿using AvaFFmpegPlayer.FFmpeg;
using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.Components;

public sealed class SubtitleComponent(MediaContainer container) : MediaComponent(container)
{
    public RescalerContext ConvertContext { get; } = new();

    public override AVMediaType MediaType => AVMediaType.AVMEDIA_TYPE_SUBTITLE;

    public override string WantedCodecName => Container.Options.AudioForcedCodecName;

    protected override FrameStore CreateFrameQueue() => new(Packets, Constants.SubtitleFrameQueueCapacity, false);

    private int DecodeSubtitleInto(FFSubtitle frame) => DecodeFrame(null, frame);

    protected override void DecodingThreadMethod()
    {
        while (true)
        {
            var frame = new FFSubtitle();
            var gotSubtitle = DecodeSubtitleInto(frame);
            if (gotSubtitle < 0)
            {
                frame.Release();
                break;
            }
            else if (gotSubtitle == 0 || frame.Format != 0)
            {
                frame.Release();
                continue;
            }

            if (!Frames.LeaseFrameForWriting(out var targetFrame))
            {
                frame.Release();
                break;
            }

            var frameTime = frame.Pts.IsValidPts() ? frame.Pts / Clock.TimeBaseMicros : 0;

            // now we can update the picture count
            targetFrame.Update(frame, CodecContext, PacketGroupIndex, frameTime);
            Frames.EnqueueLeasedFrame();
        }
    }
}
