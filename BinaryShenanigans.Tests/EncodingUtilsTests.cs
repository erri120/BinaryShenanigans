using System;
using System.Text;
using Xunit;

namespace BinaryShenanigans.Tests
{
    public class EncodingUtilsTests
    {
        private static readonly Encoding Utf8Encoding = new UTF8Encoding(false);
        private static readonly byte[] Utf8Bytes = { 0xE6, 0x97, 0xA5, 0xE6, 0x9C, 0xAC, 0xE8, 0xAA, 0x9E };

        private static readonly Encoding Utf32Encoding = Encoding.UTF32;
        private static readonly byte[] Utf32Bytes = { 0xE5, 0x65, 0x0, 0x0, 0x2C, 0x67, 0x0, 0x0, 0x9E, 0x8A, 0x0, 0x0 };

        private const string ResultString = "日本語";

        [Fact]
        public void TestConvertFromCharToByte()
        {
            var charSpan = ResultString.AsSpan();
            var utf8Bytes = EncodingUtils.ConvertFromCharToByte(charSpan, Utf8Encoding);
            AssertEqualSpans(new ReadOnlySpan<byte>(Utf8Bytes, 0, Utf8Bytes.Length), utf8Bytes);
        }

        [Fact]
        public void TestConvertFromByteToChar()
        {
            var charSpan = EncodingUtils.ConvertFromByteToChar(new ReadOnlySpan<byte>(Utf8Bytes, 0, Utf8Bytes.Length), Utf8Encoding);
            AssertEqualSpans(ResultString.AsSpan(), charSpan);
        }

        [Fact]
        public void TestConvertEncodings()
        {
            var utf32Bytes = EncodingUtils.ConvertEncodings(new ReadOnlySpan<byte>(Utf8Bytes, 0, Utf8Bytes.Length), Utf8Encoding, Utf32Encoding);
            AssertEqualSpans(Utf32Bytes, utf32Bytes);
        }

        private static void AssertEqualSpans<T>(ReadOnlySpan<T> expected, ReadOnlySpan<T> actual)
        {
            Assert.Equal(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}
