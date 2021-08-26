using System;
using System.Buffers.Binary;
using BenchmarkDotNet.Attributes;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class SpanMemoryArraySegmentBenchmarks
    {
        private readonly byte[] _buffer;
        private readonly Memory<byte> _memoryOverBuffer;
        private readonly ArraySegment<byte> _arraySegmentOverBuffer;

        public SpanMemoryArraySegmentBenchmarks()
        {
            _buffer = new byte[256];
            _memoryOverBuffer = new Memory<byte>(_buffer, 0, _buffer.Length);
            _arraySegmentOverBuffer = new ArraySegment<byte>(_buffer, 0, _buffer.Length);
        }
        
        [Benchmark]
        public Span<byte> SpanCreation()
        {
            return new Span<byte>(_buffer, 0, 128);
        }

        [Benchmark]
        public Memory<byte> MemoryCreation()
        {
            return new Memory<byte>(_buffer, 0, 128);
        }

        [Benchmark]
        public ArraySegment<byte> ArraySegmentCreation()
        {
            return new ArraySegment<byte>(_buffer, 0, 128);
        }

        [Benchmark]
        public Span<byte> MemoryToSpan()
        {
            return _memoryOverBuffer.Span;
        }

        [Benchmark]
        public Span<byte> ArraySegmentToSpan()
        {
            return new Span<byte>(_arraySegmentOverBuffer.Array, 0, _arraySegmentOverBuffer.Count);
        }

        [Benchmark]
        public ReadOnlySpan<byte> ArraySegmentToReadOnlySpan()
        {
            return new ReadOnlySpan<byte>(_arraySegmentOverBuffer.Array, 0, _arraySegmentOverBuffer.Count);
        }

        [Benchmark]
        public uint ReadFromSpan()
        {
            var span = new ReadOnlySpan<byte>(_buffer, 0, _buffer.Length);
            return BinaryPrimitives.ReadUInt32LittleEndian(span);
        }

        [Benchmark]
        public uint ReadFromMemorySpan()
        {
            Span<byte> span = _memoryOverBuffer.Span;
            return BinaryPrimitives.ReadUInt32LittleEndian(span);
        }

        [Benchmark]
        public uint ReadFromArraySegmentSpan()
        {
            var span = new ReadOnlySpan<byte>(_arraySegmentOverBuffer.Array, 0, _arraySegmentOverBuffer.Count);
            return BinaryPrimitives.ReadUInt32LittleEndian(span);
        }
    }
}