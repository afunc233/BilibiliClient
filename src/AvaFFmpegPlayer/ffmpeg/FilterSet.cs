using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FilterSet(FFFilterGraph parent) : NativeChildSet<FFFilterGraph, FFFilterContext>(parent)
{
    public override FFFilterContext this[int index]
    {
        get => new(Parent.Target->filters[index]);
        set => Parent.Target->filters[index] = value.Target;
    }

    public override int Count => Convert.ToInt32(Parent.Target->nb_filters);
}
