using System.Text;
using Xunit;

namespace BinaryShenanigans.Tests.Reader
{
    public abstract class AReaderTest
    {
        [Theory]
        [InlineData(short.MaxValue, true)]
        [InlineData(short.MinValue, true)]
        [InlineData(short.MaxValue / 2, true)]
        [InlineData(short.MinValue / 2, true)]
        [InlineData(short.MaxValue, false)]
        [InlineData(short.MinValue, false)]
        [InlineData(short.MaxValue / 2, false)]
        [InlineData(short.MinValue / 2, false)]
        public abstract void TestReadInt16(short value, bool littleEndian);

        [Theory]
        [InlineData(int.MaxValue, true)]
        [InlineData(int.MinValue, true)]
        [InlineData(int.MaxValue / 2, true)]
        [InlineData(int.MinValue / 2, true)]
        [InlineData(int.MaxValue, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue / 2, false)]
        [InlineData(int.MinValue / 2, false)]
        public abstract void TestReadInt32(int value, bool littleEndian);

        [Theory]
        [InlineData(long.MaxValue, true)]
        [InlineData(long.MinValue, true)]
        [InlineData(long.MaxValue / 2, true)]
        [InlineData(long.MinValue / 2, true)]
        [InlineData(long.MaxValue, false)]
        [InlineData(long.MinValue, false)]
        [InlineData(long.MaxValue / 2, false)]
        [InlineData(long.MinValue / 2, false)]
        public abstract void TestReadInt64(long value, bool littleEndian);

        [Theory]
        [InlineData(ushort.MaxValue, true)]
        [InlineData(ushort.MinValue, true)]
        [InlineData(ushort.MaxValue / 2, true)]
        [InlineData(ushort.MinValue / 2, true)]
        [InlineData(ushort.MaxValue, false)]
        [InlineData(ushort.MinValue, false)]
        [InlineData(ushort.MaxValue / 2, false)]
        [InlineData(ushort.MinValue / 2, false)]
        public abstract void TestReadUInt16(ushort value, bool littleEndian);

        [Theory]
        [InlineData(uint.MaxValue, true)]
        [InlineData(uint.MinValue, true)]
        [InlineData(uint.MaxValue / 2, true)]
        [InlineData(uint.MaxValue, false)]
        [InlineData(uint.MinValue, false)]
        [InlineData(uint.MaxValue / 2, false)]
        public abstract void TestReadUInt32(uint value, bool littleEndian);

        [Theory]
        [InlineData(ulong.MaxValue, true)]
        [InlineData(ulong.MinValue, true)]
        [InlineData(ulong.MaxValue / 2, true)]
        [InlineData(ulong.MaxValue, false)]
        [InlineData(ulong.MinValue, false)]
        [InlineData(ulong.MaxValue / 2, false)]
        public abstract void TestReadUInt64(ulong value, bool littleEndian);

        [Theory]
        [InlineData(double.MaxValue, true)]
        [InlineData(double.MinValue, true)]
        [InlineData(double.MaxValue / 2, true)]
        [InlineData(double.MinValue / 2, true)]
        [InlineData(double.MaxValue, false)]
        [InlineData(double.MinValue, false)]
        [InlineData(double.MaxValue / 2, false)]
        [InlineData(double.MinValue / 2, false)]
        public abstract void TestReadDouble(double value, bool littleEndian);

        [Theory]
        [InlineData(float.MaxValue, true)]
        [InlineData(float.MinValue, true)]
        [InlineData(float.MaxValue / 2, true)]
        [InlineData(float.MinValue / 2, true)]
        [InlineData(float.MaxValue, false)]
        [InlineData(float.MinValue, false)]
        [InlineData(float.MaxValue / 2, false)]
        [InlineData(float.MinValue / 2, false)]
        public abstract void TestReadSingle(float value, bool littleEndian);

#if NET6_0_OR_GREATER
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public abstract void TestReadHalfMaxValue(bool littleEndian);

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public abstract void TestReadHalfMinValue(bool littleEndian);
#endif

        [Theory]
        [InlineData("Hello World!")]
        [InlineData("日本語")]
        public abstract void TestReadString(string value);

        [Theory]
        [InlineData("Hello World!\0")]
        [InlineData("日本語\0")]
        public abstract void TestReadStringNullTerminated(string value);
    }
}
