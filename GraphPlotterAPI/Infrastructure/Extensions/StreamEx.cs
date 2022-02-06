namespace System.IO;

internal static class StreamEx
{
    public static Stream AsAsyncStream(this Stream stream) => stream switch
    {
        StreamAsyncWrapper wrapper => wrapper,
        _ => new StreamAsyncWrapper(stream)
    };
}

internal class StreamAsyncWrapper : Stream
{
    private readonly Stream _BaseStream;

    public StreamAsyncWrapper(Stream BaseStream) => _BaseStream = BaseStream; public override bool CanRead => _BaseStream.CanRead;

    public override bool CanSeek => _BaseStream.CanSeek;

    public override bool CanWrite => _BaseStream.CanWrite;

    public override long Length => _BaseStream.Length;

    public override long Position
    {
        get => _BaseStream.Position;
        set => _BaseStream.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return _BaseStream.ReadAsync(buffer, offset, count).Result;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _BaseStream.WriteAsync(buffer, offset, count).Wait();
    }

    public override void Flush() => _BaseStream.Flush();

    public override long Seek(long offset, SeekOrigin origin) => _BaseStream.Seek(offset, origin);

    public override void SetLength(long value) => _BaseStream.SetLength(value);
}