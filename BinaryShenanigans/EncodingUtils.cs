using System;
using System.Text;
using JetBrains.Annotations;

namespace BinaryShenanigans
{
    [PublicAPI]
    public static class EncodingUtils
    {
        public static ReadOnlySpan<byte> ConvertFromCharToByte(ReadOnlySpan<char> input, Encoding encoding)
        {
            var bytes = new byte[encoding.GetMaxByteCount(input.Length)];
            var byteCount = encoding.GetBytes(input, new Span<byte>(bytes, 0, bytes.Length));
            return new ReadOnlySpan<byte>(bytes, 0, byteCount);
        }

        public static ReadOnlySpan<char> ConvertFromByteToChar(ReadOnlySpan<byte> input, Encoding encoding)
        {
            var chars = new char[encoding.GetMaxCharCount(input.Length)];
            var charCount = encoding.GetChars(input, new Span<char>(chars, 0, chars.Length));
            return new ReadOnlySpan<char>(chars, 0, charCount);
        }

        public static ReadOnlySpan<byte> ConvertEncodings(ReadOnlySpan<byte> input, Encoding inputEncoding, Encoding outputEncoding)
        {
            if (inputEncoding.Equals(outputEncoding))
                throw new ArgumentException("Output Encoding must not be the same as the Input Encoding!", nameof(outputEncoding));

            var maxCharCount = inputEncoding.GetMaxCharCount(input.Length);
            var chars = input.Length > 1024 ? new char[maxCharCount] : stackalloc char[maxCharCount];
            var charCount = inputEncoding.GetChars(input, chars);

            var bytes = new byte[outputEncoding.GetMaxByteCount(charCount)];
            var byteCount = outputEncoding.GetBytes(chars[..charCount], new Span<byte>(bytes, 0, bytes.Length));

            return new ReadOnlySpan<byte>(bytes, 0, byteCount);
        }
    }
}
