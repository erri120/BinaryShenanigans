using System;
using System.Buffers.Binary;
using BenchmarkDotNet.Attributes;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class BinaryReaderExtensionsStreamBenchmarks
    {
        [Benchmark(Description = "ReadInt16 with exposed MemoryStream buffer")]
        public short ReadInt16WithExposedMemoryStreamBuffer()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short));
            return br.ReadInt16();
        }

        [Benchmark(Description = "ReadInt16 with unexposed MemoryStream buffer")]
        public short ReadInt16WithUnexposedMemoryStreamBuffer()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short),
                false);
            return br.ReadInt16();
        }
        
        [Benchmark(Description = "ReadInt16BE with exposed MemoryStream buffer")]
        public short ReadInt16BEWithExposedMemoryStreamBuffer()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short));
            return br.ReadInt16BE();
        }

        [Benchmark(Description = "ReadInt16BE with unexposed MemoryStream buffer")]
        public short ReadInt16BEWithUnexposedMemoryStreamBuffer()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short),
                false);
            return br.ReadInt16BE();
        }

        [Benchmark(Description = "ReadInt16 with FileStream")]
        public short ReadInt16WithFileStream()
        {
            using var br = Utils.SetupBinaryReaderWithFileStream(
                buffer => BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short));
            return br.ReadInt16();
        }
        
        [Benchmark(Description = "ReadInt16BE with FileStream")]
        public short ReadInt16BEWithFileStream()
        {
            using var br = Utils.SetupBinaryReaderWithFileStream(
                buffer => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short));
            return br.ReadInt16BE();
        }
    }
}