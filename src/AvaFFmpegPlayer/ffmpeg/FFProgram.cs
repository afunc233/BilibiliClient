using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class FFProgram(AVProgram* target) : NativeReference<AVProgram>(target)
{
    public int StreamIndexCount => Convert.ToInt32(Target->nb_stream_indexes);

    public IReadOnlyList<int> StreamIndices
    {
        get
        {
            var result = new List<int>(StreamIndexCount);
            for (var i = 0; i < StreamIndexCount; i++)
                result.Add(Convert.ToInt32(Target->stream_index[i]));

            return result;
        }
    }
}
