﻿using Xunit;

namespace BinaryShenanigans.Tests.Writer
{
    public abstract class AWriterTest
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
        public abstract void TestWriteInt16(short value, bool littleEndian);
        
        [Theory]
        [InlineData(int.MaxValue, true)]
        [InlineData(int.MinValue, true)]
        [InlineData(int.MaxValue / 2, true)]
        [InlineData(int.MinValue / 2, true)]
        [InlineData(int.MaxValue, false)]
        [InlineData(int.MinValue, false)]
        [InlineData(int.MaxValue / 2, false)]
        [InlineData(int.MinValue / 2, false)]
        public abstract void TestWriteInt32(int value, bool littleEndian);
        
        [Theory]
        [InlineData(long.MaxValue, true)]
        [InlineData(long.MinValue, true)]
        [InlineData(long.MaxValue / 2, true)]
        [InlineData(long.MinValue / 2, true)]
        [InlineData(long.MaxValue, false)]
        [InlineData(long.MinValue, false)]
        [InlineData(long.MaxValue / 2, false)]
        [InlineData(long.MinValue / 2, false)]
        public abstract void TestWriteInt64(long value, bool littleEndian);
        
        [Theory]
        [InlineData(ushort.MaxValue, true)]
        [InlineData(ushort.MinValue, true)]
        [InlineData(ushort.MaxValue / 2, true)]
        [InlineData(ushort.MinValue / 2, true)]
        [InlineData(ushort.MaxValue, false)]
        [InlineData(ushort.MinValue, false)]
        [InlineData(ushort.MaxValue / 2, false)]
        [InlineData(ushort.MinValue / 2, false)]
        public abstract void TestWriteUInt16(ushort value, bool littleEndian);
        
        [Theory]
        [InlineData(uint.MaxValue, true)]
        [InlineData(uint.MinValue, true)]
        [InlineData(uint.MaxValue / 2, true)]
        [InlineData(uint.MaxValue, false)]
        [InlineData(uint.MinValue, false)]
        [InlineData(uint.MaxValue / 2, false)]
        public abstract void TestWriteUInt32(uint value, bool littleEndian);
        
        [Theory]
        [InlineData(ulong.MaxValue, true)]
        [InlineData(ulong.MinValue, true)]
        [InlineData(ulong.MaxValue / 2, true)]
        [InlineData(ulong.MaxValue, false)]
        [InlineData(ulong.MinValue, false)]
        [InlineData(ulong.MaxValue / 2, false)]
        public abstract void TestWriteUInt64(ulong value, bool littleEndian);
        
        [Theory]
        [InlineData(double.MaxValue, true)]
        [InlineData(double.MinValue, true)]
        [InlineData(double.MaxValue / 2, true)]
        [InlineData(double.MinValue / 2, true)]
        [InlineData(double.MaxValue, false)]
        [InlineData(double.MinValue, false)]
        [InlineData(double.MaxValue / 2, false)]
        [InlineData(double.MinValue / 2, false)]
        public abstract void TestWriteDouble(double value, bool littleEndian);
        
        [Theory]
        [InlineData(float.MaxValue, true)]
        [InlineData(float.MinValue, true)]
        [InlineData(float.MaxValue / 2, true)]
        [InlineData(float.MinValue / 2, true)]
        [InlineData(float.MaxValue, false)]
        [InlineData(float.MinValue, false)]
        [InlineData(float.MaxValue / 2, false)]
        [InlineData(float.MinValue / 2, false)]
        public abstract void TestWriteSingle(float value, bool littleEndian);
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public abstract void TestWriteHalfMaxValue(bool littleEndian);
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public abstract void TestWriteHalfMinValue(bool littleEndian);
    }
}