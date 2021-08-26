using System;
using System.Buffers.Binary;
using BenchmarkDotNet.Attributes;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    [MemoryDiagnoser]
    public class BinaryReaderExtensionsBenchmarks
    {
        [Benchmark]
        public short ReadInt16()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short));
            return br.ReadInt16();
        }

        [Benchmark]
        public short ReadInt16BE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), short.MaxValue),
                sizeof(short));
            return br.ReadInt16BE();
        }
        
        [Benchmark]
        public int ReadInt32()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(), int.MaxValue),
                sizeof(int));
            return br.ReadInt32();
        }

        [Benchmark]
        public int ReadInt32BE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt32BigEndian(buffer.AsSpan(), int.MaxValue),
                sizeof(int));
            return br.ReadInt32BE();
        }
        
        [Benchmark]
        public long ReadInt64()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(), long.MaxValue),
                sizeof(long));
            return br.ReadInt64();
        }

        [Benchmark]
        public long ReadInt64BE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt64BigEndian(buffer.AsSpan(), long.MaxValue),
                sizeof(long));
            return br.ReadInt64BE();
        }

        [Benchmark]
        public ushort ReadUInt16()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(), ushort.MaxValue),
                sizeof(ushort));
            return br.ReadUInt16();
        }

        [Benchmark]
        public ushort ReadUInt16BE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(), ushort.MaxValue),
                sizeof(ushort));
            return br.ReadUInt16BE();
        }
        
        [Benchmark]
        public uint ReadUInt32()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan(), uint.MaxValue),
                sizeof(uint));
            return br.ReadUInt32();
        }

        [Benchmark]
        public uint ReadUInt32BE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan(), uint.MaxValue),
                sizeof(uint));
            return br.ReadUInt32BE();
        }
        
        [Benchmark]
        public ulong ReadUInt64()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteUInt64LittleEndian(buffer.AsSpan(), ulong.MaxValue),
                sizeof(ulong));
            return br.ReadUInt64();
        }

        [Benchmark]
        public ulong ReadUInt64BE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteUInt64BigEndian(buffer.AsSpan(), ulong.MaxValue),
                sizeof(ulong));
            return br.ReadUInt64BE();
        }

        [Benchmark]
        public double ReadDouble()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteDoubleLittleEndian(buffer.AsSpan(), double.MaxValue),
                sizeof(double));
            return br.ReadDouble();
        }
        
        [Benchmark]
        public double ReadDoubleBE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteDoubleBigEndian(buffer.AsSpan(), double.MaxValue),
                sizeof(double));
            return br.ReadDoubleBE();
        }
        
        [Benchmark]
        public float ReadSingle()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteSingleLittleEndian(buffer.AsSpan(), float.MaxValue),
                sizeof(float));
            return br.ReadSingle();
        }
        
        [Benchmark]
        public float ReadSingleBE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteSingleBigEndian(buffer.AsSpan(), float.MaxValue),
                sizeof(float));
            return br.ReadSingleBE();
        }
        
        [Benchmark]
        public Half ReadHalf()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteHalfLittleEndian(buffer.AsSpan(), Half.MaxValue),
                sizeof(ushort));
            return br.ReadHalf();
        }
        
        [Benchmark]
        public Half ReadHalfBE()
        {
            using var br = Utils.SetupBinaryReader(
                buffer => BinaryPrimitives.WriteHalfBigEndian(buffer.AsSpan(), Half.MaxValue),
                sizeof(ushort));
            return br.ReadHalf();
        }
    }
}