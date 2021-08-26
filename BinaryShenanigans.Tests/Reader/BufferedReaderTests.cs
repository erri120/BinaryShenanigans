using System;
using System.Buffers.Binary;
using BinaryShenanigans.Reader;
using Xunit;

namespace BinaryShenanigans.Tests.Reader
{
    public class BufferedReaderTests : AReaderTest
    {
        public override void TestReadInt16(short value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(short),
                littleEndian
                    ? x => BinaryPrimitives.WriteInt16LittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteInt16BigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadInt16());
        }
        
        public override void TestReadInt32(int value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(int),
                littleEndian
                    ? x => BinaryPrimitives.WriteInt32LittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteInt32BigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadInt32());
        }
        
        public override void TestReadInt64(long value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(long),
                littleEndian
                    ? x => BinaryPrimitives.WriteInt64LittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteInt64BigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadInt64());
        }
        
        public override void TestReadUInt16(ushort value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(ushort),
                littleEndian
                    ? x => BinaryPrimitives.WriteUInt16LittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteUInt16BigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadUInt16());
        }
        
        public override void TestReadUInt32(uint value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(uint),
                littleEndian
                    ? x => BinaryPrimitives.WriteUInt32LittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteUInt32BigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadUInt32());
        }
        
        public override void TestReadUInt64(ulong value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(ulong),
                littleEndian
                    ? x => BinaryPrimitives.WriteUInt64LittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteUInt64BigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadUInt64());
        }
        
        public override void TestReadDouble(double value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(double),
                littleEndian
                    ? x => BinaryPrimitives.WriteDoubleLittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteDoubleBigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadDouble());
        }
        
        public override void TestReadSingle(float value, bool littleEndian)
        {
            TestBufferedReader(
                sizeof(float),
                littleEndian
                    ? x => BinaryPrimitives.WriteSingleLittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteSingleBigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadSingle());
        }

        public override void TestReadHalfMaxValue(bool littleEndian)
        {
            var value = Half.MaxValue;
            TestBufferedReader(
                Constants.HalfSize,
                littleEndian
                    ? x => BinaryPrimitives.WriteHalfLittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteHalfBigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadHalf());
        }
        
        public override void TestReadHalfMinValue(bool littleEndian)
        {
            var value = Half.MinValue;
            TestBufferedReader(
                Constants.HalfSize,
                littleEndian
                    ? x => BinaryPrimitives.WriteHalfLittleEndian(x.AsSpan(), value)
                    : x => BinaryPrimitives.WriteHalfBigEndian(x.AsSpan(), value),
                littleEndian,
                value,
                reader => reader.ReadHalf());
        }

        private static void TestBufferedReader<T>(int size, Action<byte[]> writeValue, bool littleEndian,
            T expectedValue, Func<BufferedReader, T> readFunc)
        {
            var buffer = new byte[size];
            writeValue(buffer);
            var reader = new BufferedReader(buffer, littleEndian);
            var actualValue = readFunc(reader);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}