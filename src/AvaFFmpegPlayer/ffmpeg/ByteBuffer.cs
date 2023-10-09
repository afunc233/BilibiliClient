using AvaFFmpegPlayer.Primitives;

namespace AvaFFmpegPlayer.FFmpeg;

public sealed unsafe class ByteBuffer : CountedReference<byte>
{
    public ByteBuffer(ulong length, [CallerFilePath] string filePath = default,
        [CallerLineNumber] int? lineNumber = default)
        : base(filePath, lineNumber)
    {
        var pointer = (byte*)ffmpeg.av_mallocz(length);
        Update(pointer);
        Length = length;
    }

    public ulong Length { get; private set; }

    /// <summary>
    /// 重新分配
    /// </summary>
    /// <param name="original"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static ByteBuffer Reallocate(ByteBuffer original, ulong length)
    {
        if (original.IsNull() || original.Length < length)
        {
            original?.Release();
            return new(length);
        }

        return original;
    }

    public void Write(byte* source, int length)
    {
        var maxLength = Math.Min(Convert.ToInt32(Length), length);
        Buffer.MemoryCopy(source, Target, maxLength, maxLength);
    }

    protected override void ReleaseInternal(byte* pointer)
    {
        ffmpeg.av_free(pointer);
        Length = 0;
    }
}