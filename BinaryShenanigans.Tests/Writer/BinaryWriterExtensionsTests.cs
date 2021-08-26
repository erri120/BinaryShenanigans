using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using BinaryShenanigans.Writer;
using Xunit;

namespace BinaryShenanigans.Tests.Writer
{
    public class BinaryWriterExtensionsTests
    {
        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue / 2)]
        [InlineData(short.MinValue / 2)]
        public void TestWriteInt16(short value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteInt16BE(value),
                buffer => BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan()),
                sizeof(short),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan()),
                sizeof(short),
                value);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue / 2)]
        [InlineData(int.MinValue / 2)]
        public void TestWriteInt32(int value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteInt32BE(value),
                buffer => BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan()),
                sizeof(int),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan()),
                sizeof(int),
                value);
        }

        [Theory]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue / 2)]
        [InlineData(long.MinValue / 2)]
        public void TestWriteInt64(long value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteInt64BE(value),
                buffer => BinaryPrimitives.ReadInt64BigEndian(buffer.AsSpan()),
                sizeof(long),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadInt64BigEndian(buffer.AsSpan()),
                sizeof(long),
                value);
        }

        [Theory]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue / 2)]
        public void TestWriteUInt16(ushort value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteUInt16BE(value),
                buffer => BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan()),
                sizeof(ushort),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan()),
                sizeof(ushort),
                value);
        }

        [Theory]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue / 2)]
        public void TestWriteUInt32(uint value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteUInt32BE(value),
                buffer => BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan()),
                sizeof(uint),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan()),
                sizeof(uint),
                value);
        }

        [Theory]
        [InlineData(ulong.MaxValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue / 2)]
        public void TestWriteUInt64(ulong value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteUInt64BE(value),
                buffer => BinaryPrimitives.ReadUInt64BigEndian(buffer.AsSpan()),
                sizeof(ulong),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadUInt64BigEndian(buffer.AsSpan()),
                sizeof(ulong),
                value);
        }

        [Theory]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue / 2)]
        [InlineData(double.MinValue / 2)]
        public void TestWriteDoubleBE(double value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteDoubleBE(value),
                buffer => BinaryPrimitivesMethods.ReadDoubleBigEndian(buffer.AsSpan()),
                sizeof(double),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitivesMethods.ReadDoubleBigEndian(buffer.AsSpan()),
                sizeof(double),
                value);
        }

        [Theory]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue / 2)]
        [InlineData(float.MinValue / 2)]
        public void TestWriteSingleBE(float value)
        {
            TestBinaryWriterExtension(
                bw => bw.WriteSingleBE(value),
                buffer => BinaryPrimitivesMethods.ReadSingleBigEndian(buffer.AsSpan()),
                sizeof(float),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitivesMethods.ReadSingleBigEndian(buffer.AsSpan()),
                sizeof(float),
                value);
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void TestWriteHalfBEMaxValue()
        {
            var value = Half.MaxValue;
            TestBinaryWriterExtension(
                bw => bw.WriteHalfBE(value),
                buffer => BinaryPrimitives.ReadHalfBigEndian(buffer.AsSpan()),
                sizeof(ushort),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadHalfBigEndian(buffer.AsSpan()),
                sizeof(ushort),
                value);
        }

        [Fact]
        public void TestWriteHalfBEMinValue()
        {
            var value = Half.MinValue;
            TestBinaryWriterExtension(
                bw => bw.WriteHalfBE(value),
                buffer => BinaryPrimitives.ReadHalfBigEndian(buffer.AsSpan()),
                sizeof(ushort),
                value);
            TestBinaryWriterExtension(
                bw => bw.WriteBE(value),
                buffer => BinaryPrimitives.ReadHalfBigEndian(buffer.AsSpan()),
                sizeof(ushort),
                value);
        }
#endif

        private static void TestBinaryWriterExtension<T>(Action<System.IO.BinaryWriter> writeValue, Func<byte[], T> readValue,
            int size, T expectedValue)
        {
            var buffer = new byte[size];

            using var ms = new MemoryStream(buffer, 0, size, true, true);
            using var bw = new System.IO.BinaryWriter(ms, Encoding.UTF8, false);

            writeValue(bw);
            var actualValue = readValue(buffer);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
