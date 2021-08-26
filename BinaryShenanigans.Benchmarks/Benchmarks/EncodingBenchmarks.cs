using System;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class EncodingBenchmarks
    {
        private const string InputString = "日本語";

        private Encoding Encoding { get; }

        public EncodingBenchmarks()
        {
            Encoding = Encoding.UTF8;
        }

        [Benchmark]
        public ReadOnlySpan<byte> WithGetMaxByteCount()
        {
            var input = InputString.AsSpan();
            var bytes = new byte[Encoding.GetMaxByteCount(input.Length)];
            var byteCount = Encoding.GetBytes(input, bytes.AsSpan());
            return new ReadOnlySpan<byte>(bytes, 0, byteCount);
        }

        [Benchmark]
        public ReadOnlySpan<byte> WithGetByteCount()
        {
            var input = InputString.AsSpan();
            var expectedByteCount = Encoding.GetByteCount(input);
            var bytes = new byte[expectedByteCount];
            var actualByteCount = Encoding.GetBytes(input, bytes.AsSpan());
            return new ReadOnlySpan<byte>(bytes, 0, actualByteCount);
        }
    }
}
