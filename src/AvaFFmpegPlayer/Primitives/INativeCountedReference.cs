namespace AvaFFmpegPlayer.Primitives;

public interface INativeCountedReference : INativeReference
{
    ulong ObjectId { get; }

    void Release();
}
