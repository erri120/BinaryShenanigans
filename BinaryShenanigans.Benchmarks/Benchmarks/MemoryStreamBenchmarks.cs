using System;
using System.IO;
using BenchmarkDotNet.Attributes;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class MemoryStreamBenchmarks
    {
        private readonly byte[] _buffer;

        public MemoryStreamBenchmarks()
        {
            _buffer = new byte[256];
        }

        [Benchmark]
        public byte[] GetBuffer()
        {
            using var ms = new MemoryStream(_buffer, 0, _buffer.Length, false, true);
            return ms.GetBuffer();
        }

        [Benchmark]
        public ArraySegment<byte> TryGetBuffer()
        {
            using var ms = new MemoryStream(_buffer, 0, _buffer.Length, false, true);
            ms.TryGetBuffer(out var arraySegment);
            return arraySegment;
        }
        
        [Benchmark]
        public byte[] TryGetBufferFromArraySegment()
        {
            using var ms = new MemoryStream(_buffer, 0, _buffer.Length, false, true);
            ms.TryGetBuffer(out var arraySegment);
            return arraySegment.Array!;
        }

        [Benchmark]
        public long SetPosition()
        {
            using var ms = new MemoryStream(_buffer, 0, _buffer.Length, false, true);
            ms.Position += 128;
            return ms.Position;
        }
        
        [Benchmark]
        public long Seek()
        {
            using var ms = new MemoryStream(_buffer, 0, _buffer.Length, false, true);
            ms.Seek(128, SeekOrigin.Current);
            return ms.Position;
        }
    }
}