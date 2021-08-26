using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace BinaryShenanigans.Writer
{
    [PublicAPI]
    public class SpanWriter
    {
        private int _pos;
        private readonly int _count;

        public bool LittleEndian;

        public SpanWriter(int start, int count, bool littleEndian = true)
        {
            _pos = start;
            _count = count;

            LittleEndian = littleEndian;
        }

        public void Write(Span<byte> span, short value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, ushort value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, int value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, uint value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, long value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, ulong value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, double value) => Write(span, value, LittleEndian);
        public void Write(Span<byte> span, float value) => Write(span, value, LittleEndian);
#if NET6_0_OR_GREATER
        public void Write(Span<byte> span, Half value) => Write(span, value, LittleEndian);
#endif

        public void WriteLE(Span<byte> span, short value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, ushort value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, int value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, uint value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, long value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, ulong value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, double value) => Write(span, value, true);
        public void WriteLE(Span<byte> span, float value) => Write(span, value, true);
#if NET6_0_OR_GREATER
        public void WriteLE(Span<byte> span, Half value) => Write(span, value, true);
#endif

        public void WriteBE(Span<byte> span, short value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, ushort value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, int value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, uint value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, long value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, ulong value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, double value) => Write(span, value, false);
        public void WriteBE(Span<byte> span, float value) => Write(span, value, false);
#if NET6_0_OR_GREATER
        public void WriteBE(Span<byte> span, Half value) => Write(span, value, false);
#endif

        public void Write(Span<byte> span, short value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteInt16LittleEndian(GetSlice(span, sizeof(short)), value);
            else
                BinaryPrimitives.WriteInt16BigEndian(GetSlice(span, sizeof(short)), value);
        }

        public void Write(Span<byte> span, int value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteInt32LittleEndian(GetSlice(span, sizeof(int)), value);
            else
                BinaryPrimitives.WriteInt32BigEndian(GetSlice(span, sizeof(int)), value);
        }

        public void Write(Span<byte> span, long value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteInt64LittleEndian(GetSlice(span, sizeof(long)), value);
            else
                BinaryPrimitives.WriteInt64BigEndian(GetSlice(span, sizeof(long)), value);
        }

        public void Write(Span<byte> span, ushort value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteUInt16LittleEndian(GetSlice(span, sizeof(ushort)), value);
            else
                BinaryPrimitives.WriteUInt16BigEndian(GetSlice(span, sizeof(ushort)), value);
        }

        public void Write(Span<byte> span, uint value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteUInt32LittleEndian(GetSlice(span, sizeof(uint)), value);
            else
                BinaryPrimitives.WriteUInt32BigEndian(GetSlice(span, sizeof(uint)), value);
        }

        public void Write(Span<byte> span, ulong value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteUInt64LittleEndian(GetSlice(span, sizeof(ulong)), value);
            else
                BinaryPrimitives.WriteUInt64BigEndian(GetSlice(span, sizeof(ulong)), value);
        }

        public void Write(Span<byte> span, double value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitivesMethods.WriteDoubleLittleEndian(GetSlice(span, sizeof(double)), value);
            else
                BinaryPrimitivesMethods.WriteDoubleBigEndian(GetSlice(span, sizeof(double)), value);
        }

        public void Write(Span<byte> span, float value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitivesMethods.WriteSingleLittleEndian(GetSlice(span, sizeof(float)), value);
            else
                BinaryPrimitivesMethods.WriteSingleBigEndian(GetSlice(span, sizeof(float)), value);
        }

#if NET6_0_OR_GREATER
        public void Write(Span<byte> span, Half value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteHalfLittleEndian(GetSlice(span, Constants.HalfSize), value);
            else
                BinaryPrimitives.WriteHalfBigEndian(GetSlice(span, Constants.HalfSize), value);
        }
#endif

        public void Write(Span<byte> span, string value, Encoding encoding, bool nullTerminated = false)
        {
            value = nullTerminated ? value + '\0' : value;
            var bytes = EncodingUtils.ConvertFromCharToByte(value.AsSpan(), encoding);
            var slice = GetSlice(span, bytes.Length);
            bytes.CopyTo(slice);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> GetSlice(Span<byte> span, int size)
        {
            if (_pos + size > _count)
                throw new EndOfStreamException();

            var slice = span.Slice(_pos, size);
            _pos += size;
            return slice;
        }
    }
}
