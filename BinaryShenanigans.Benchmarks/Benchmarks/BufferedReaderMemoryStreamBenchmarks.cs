using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using BenchmarkDotNet.Attributes;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class BufferedReaderMemoryStreamBenchmarks
    {
        private readonly byte[] _buffer;
        
        public BufferedReaderMemoryStreamBenchmarks()
        {
            _buffer = new byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32LittleEndian(_buffer.AsSpan(), 1337);
        }

        [Benchmark]
        public uint UsingSpanReader()
        {
            var reader = new SpanReader(0, _buffer.Length);
            return reader.ReadUInt32(new ReadOnlySpan<byte>(_buffer, 0, _buffer.Length));
        }
        
        [Benchmark]
        public uint UsingBufferedReader()
        {
            var reader = new BufferedReader(_buffer);
            return reader.ReadUInt32();
        }

        [Benchmark]
        public uint UsingMemoryStreamBinaryReader()
        {
            using var ms = new MemoryStream(_buffer, 0, _buffer.Length, false, true);
            using var br = new BinaryReader(ms, Encoding.UTF8, false);
            return br.ReadUInt32();
        }
    }
}