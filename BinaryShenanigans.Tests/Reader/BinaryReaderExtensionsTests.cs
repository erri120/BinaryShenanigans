using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using BinaryShenanigans.Reader;
using Xunit;

namespace BinaryShenanigans.Tests.Reader
{
    public class BinaryReaderExtensionsTests
    {
        [Theory]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue / 2)]
        [InlineData(short.MinValue / 2)]
        public void TestReadInt16BE(short value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), value),
                br => br.ReadInt16BE(),
                sizeof(short),
                value);
        }
        
        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue / 2)]
        [InlineData(int.MinValue / 2)]
        public void TestReadInt32BE(int value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteInt32BigEndian(buffer.AsSpan(), value),
                br => br.ReadInt32BE(),
                sizeof(int),
                value);
        }
        
        [Theory]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue / 2)]
        [InlineData(long.MinValue / 2)]
        public void TestReadInt64BE(long value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteInt64BigEndian(buffer.AsSpan(), value),
                br => br.ReadInt64BE(),
                sizeof(long),
                value);
        }

        [Theory]
        [InlineData(ushort.MaxValue)]
        [InlineData(ushort.MinValue)]
        [InlineData(ushort.MaxValue / 2)]
        public void TestReadUInt16BE(ushort value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(), value),
                br => br.ReadUInt16BE(),
                sizeof(ushort),
                value);
        }
        
        [Theory]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue / 2)]
        public void TestReadUInt32BE(uint value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan(), value),
                br => br.ReadUInt32BE(),
                sizeof(uint),
                value);
        }
        
        [Theory]
        [InlineData(ulong.MaxValue)]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue / 2)]
        public void TestReadUInt64BE(ulong value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteUInt64BigEndian(buffer.AsSpan(), value),
                br => br.ReadUInt64BE(),
                sizeof(ulong),
                value);
        }
        
        [Theory]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue / 2)]
        [InlineData(double.MinValue / 2)]
        public void TestReadDoubleBE(double value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteDoubleBigEndian(buffer.AsSpan(), value),
                br => br.ReadDoubleBE(),
                sizeof(double),
                value);
        }
        
        [Theory]
        [InlineData(float.MaxValue)]
        [InlineData(float.MinValue)]
        [InlineData(float.MaxValue / 2)]
        [InlineData(float.MinValue / 2)]
        public void TestReadSingleBE(float value)
        {
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteSingleBigEndian(buffer.AsSpan(), value),
                br => br.ReadSingleBE(),
                sizeof(float),
                value);
        }
        
        [Fact]
        public void TestReadHalfBEMaxValue()
        {
            var value = Half.MaxValue;
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteHalfBigEndian(buffer.AsSpan(), value),
                br => br.ReadHalfBE(),
                sizeof(ushort),
                value);
        }
        
        [Fact]
        public void TestReadHalfBEMinValue()
        {
            var value = Half.MinValue;
            TestBinaryReaderExtension(
                buffer => BinaryPrimitives.WriteHalfBigEndian(buffer.AsSpan(), value),
                br => br.ReadHalfBE(),
                sizeof(ushort),
                value);
        }

        [Fact]
        public void TestThrowsEndOfStreamExceptionWithMemoryStream()
        {
            using var br = SetupBinaryReader(
                buffer => BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan(), short.MinValue),
                sizeof(short));

            Assert.Throws<EndOfStreamException>(() => br.ReadInt32BE());
        }
        
        [Fact]
        public void TestThrowsEndOfStreamExceptionWithFileStream()
        {
            var buffer = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(), ushort.MaxValue);

            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, buffer);
            Assert.True(File.Exists(temp));
            
            using (var fs = File.Open(temp, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var br = new BinaryReader(fs, Encoding.UTF8, false))
            {
                Assert.Throws<EndOfStreamException>(() => br.ReadUInt32BE());
            }
            
            File.Delete(temp);
            Assert.False(File.Exists(temp));
        }

        [Fact]
        public void TestReadBEWithFileStream()
        {
            var buffer = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(), ushort.MaxValue);

            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, buffer);
            Assert.True(File.Exists(temp));
            
            using (var fs = File.Open(temp, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var br = new BinaryReader(fs, Encoding.UTF8, false))
            {
                var res = br.ReadUInt16BE();
                Assert.Equal(ushort.MaxValue, res);
            }
            
            File.Delete(temp);
            Assert.False(File.Exists(temp));
        }
        
        private static void TestBinaryReaderExtension<T>(Action<byte[]> writeValue, Func<BinaryReader, T> readValue,
            int size, T expectedValue)
        {
            using var br = SetupBinaryReader(writeValue, size);
            var actualValue = readValue(br);
            Assert.Equal(expectedValue, actualValue);
        }

        private static BinaryReader SetupBinaryReader(Action<byte[]> writeValue, int count)
        {
            var buffer = new byte[count];
            writeValue(buffer);

            var ms = new MemoryStream(buffer, 0, count, false, true);
            var br = new BinaryReader(ms, Encoding.UTF8, false);

            return br;
        }
    }
}