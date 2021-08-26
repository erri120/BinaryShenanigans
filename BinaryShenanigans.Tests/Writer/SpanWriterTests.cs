using System;
using System.Buffers.Binary;
using System.Text;
using BinaryShenanigans.Writer;
using Xunit;

namespace BinaryShenanigans.Tests.Writer
{
    public class SpanWriterTest : AWriterTest
    {
public override void TestWriteInt16(short value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(short),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadInt16LittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteInt32(int value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(int),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadInt32LittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteInt64(long value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(long),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadInt64LittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadInt64BigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteUInt16(ushort value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(ushort),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteUInt32(uint value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(uint),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteUInt64(ulong value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(ulong),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadUInt64LittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadUInt64BigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteDouble(double value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(double),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadDoubleLittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadDoubleBigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteSingle(float value, bool littleEndian)
        {
            TestBufferedWriter(
                sizeof(float),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadSingleLittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadSingleBigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteHalfMaxValue(bool littleEndian)
        {
            var value = Half.MaxValue;
            TestBufferedWriter(
                sizeof(float),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadHalfLittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadHalfBigEndian(buffer.AsSpan()),
                value);
        }

        public override void TestWriteHalfMinValue(bool littleEndian)
        {
            var value = Half.MinValue;
            TestBufferedWriter(
                sizeof(float),
                littleEndian,
                (writer, buffer) => writer.Write(buffer, value),
                littleEndian
                    ? buffer => BinaryPrimitives.ReadHalfLittleEndian(buffer.AsSpan())
                    : buffer => BinaryPrimitives.ReadHalfBigEndian(buffer.AsSpan()),
                value);
        }

        private static void TestBufferedWriter<T>(int size, bool littleEndian, Action<SpanWriter, byte[]> writeValue, Func<byte[], T> readValue, T expectedValue)
        {
            var buffer = new byte[size];
            var writer = new SpanWriter(0, size, Encoding.UTF8, littleEndian);
            writeValue(writer, buffer);
            var actualValue = readValue(buffer);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}