using System;
using System.Buffers.Binary;
using System.Text;
using BinaryShenanigans.Reader;
using Xunit;

namespace BinaryShenanigans.Tests.Reader
{
    public class SpanReaderTests : AReaderTest
    {
        public override void TestReadInt16(short value, bool littleEndian)
        {
            var buffer = new byte[sizeof(short)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadInt16(buffer.AsSpan()),
                value);
        }

        public override void TestReadInt32(int value, bool littleEndian)
        {
            var buffer = new byte[sizeof(int)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteInt32BigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadInt32(buffer.AsSpan()),
                value);
        }

        public override void TestReadInt64(long value, bool littleEndian)
        {
            var buffer = new byte[sizeof(long)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteInt64BigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadInt64(buffer.AsSpan()),
                value);
        }

        public override void TestReadUInt16(ushort value, bool littleEndian)
        {
            var buffer = new byte[sizeof(ushort)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadUInt16(buffer.AsSpan()),
                value);
        }

        public override void TestReadUInt32(uint value, bool littleEndian)
        {
            var buffer = new byte[sizeof(uint)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadUInt32(buffer.AsSpan()),
                value);
        }

        public override void TestReadUInt64(ulong value, bool littleEndian)
        {
            var buffer = new byte[sizeof(ulong)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteUInt64LittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteUInt64BigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadUInt64(buffer.AsSpan()),
                value);
        }

        public override void TestReadDouble(double value, bool littleEndian)
        {
            var buffer = new byte[sizeof(double)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitivesMethods.WriteDoubleLittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitivesMethods.WriteDoubleBigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadDouble(buffer.AsSpan()),
                value);
        }

        public override void TestReadSingle(float value, bool littleEndian)
        {
            var buffer = new byte[sizeof(float)];
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitivesMethods.WriteSingleLittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitivesMethods.WriteSingleBigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadSingle(buffer.AsSpan()),
                value);
        }

#if NET6_0_OR_GREATER
        public override void TestReadHalfMaxValue(bool littleEndian)
        {
            var buffer = new byte[Constants.HalfSize];
            var value = Half.MaxValue;
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteHalfLittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteHalfBigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadHalf(buffer.AsSpan()),
                value);
        }

        public override void TestReadHalfMinValue(bool littleEndian)
        {
            var buffer = new byte[Constants.HalfSize];
            var value = Half.MinValue;
            TestSpanReader(
                buffer.AsSpan(),
                littleEndian ? () => BinaryPrimitives.WriteHalfLittleEndian(buffer.AsSpan(), value)
                    : () => BinaryPrimitives.WriteHalfBigEndian(buffer.AsSpan(), value),
                littleEndian,
                reader => reader.ReadHalf(buffer.AsSpan()),
                value);
        }
#endif

        public override void TestReadString(string value)
        {
            var bytes = EncodingUtils.ConvertFromCharToByte(value.AsSpan(), Encoding.UTF8);
            var spanReader = new SpanReader(0, bytes.Length);
            var actualValue = spanReader.ReadString(bytes, value.Length, Encoding.UTF8);
            Assert.Equal(value, actualValue.ToString());
        }

        public override void TestReadStringNullTerminated(string value)
        {
            var bytes = EncodingUtils.ConvertFromCharToByte(value.AsSpan(), Encoding.UTF8);
            var spanReader = new SpanReader(0, bytes.Length);
            var actualValue = spanReader.ReadString(bytes, Encoding.UTF8);
            Assert.Equal(value[..^1], actualValue.ToString());
        }

        private static void TestSpanReader<T>(Span<byte> span, Action writeValue,
            bool littleEndian, Func<SpanReader, T> readValue, T expectedValue)
        {
            writeValue();
            var spanReader = new SpanReader(0, span.Length, littleEndian);
            var actualValue = readValue(spanReader);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
