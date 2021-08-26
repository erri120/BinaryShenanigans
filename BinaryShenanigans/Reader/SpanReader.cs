using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace BinaryShenanigans.Reader
{
    [PublicAPI]
    public class SpanReader
    {
        private readonly int _count;

        public bool LittleEndian;
        public int Position { get; private set; }

        public SpanReader(int start, int count, bool littleEndian = true)
        {
            Position = start;
            _count = count;

            LittleEndian = littleEndian;
        }

        public void SkipBytes(int count)
        {
            if (Position + count > _count)
                throw new EndOfStreamException();
            Position += count;
        }

        public short ReadInt16(ReadOnlySpan<byte> span) => ReadInt16(span, LittleEndian);
        public int ReadInt32(ReadOnlySpan<byte> span) => ReadInt32(span, LittleEndian);
        public long ReadInt64(ReadOnlySpan<byte> span) => ReadInt64(span, LittleEndian);
        public ushort ReadUInt16(ReadOnlySpan<byte> span) => ReadUInt16(span, LittleEndian);
        public uint ReadUInt32(ReadOnlySpan<byte> span) => ReadUInt32(span, LittleEndian);
        public ulong ReadUInt64(ReadOnlySpan<byte> span) => ReadUInt64(span, LittleEndian);
        public double ReadDouble(ReadOnlySpan<byte> span) => ReadDouble(span, LittleEndian);
        public float ReadSingle(ReadOnlySpan<byte> span) => ReadSingle(span, LittleEndian);
#if NET6_0_OR_GREATER
        public Half ReadHalf(ReadOnlySpan<byte> span) => ReadHalf(span, LittleEndian);
#endif

        public short ReadInt16LE(ReadOnlySpan<byte> span) => ReadInt16(span, true);
        public int ReadInt32LE(ReadOnlySpan<byte> span) => ReadInt32(span, true);
        public long ReadInt64LE(ReadOnlySpan<byte> span) => ReadInt64(span, true);
        public ushort ReadUInt16LE(ReadOnlySpan<byte> span) => ReadUInt16(span, true);
        public uint ReadUInt32LE(ReadOnlySpan<byte> span) => ReadUInt32(span, true);
        public ulong ReadUInt64LE(ReadOnlySpan<byte> span) => ReadUInt64(span, true);
        public double ReadDoubleLE(ReadOnlySpan<byte> span) => ReadDouble(span, true);
        public float ReadSingleLE(ReadOnlySpan<byte> span) => ReadSingle(span, true);
#if NET6_0_OR_GREATER
        public Half ReadHalfLE(ReadOnlySpan<byte> span) => ReadHalf(span, true);
#endif

        public short ReadInt16BE(ReadOnlySpan<byte> span) => ReadInt16(span, false);
        public int ReadInt32BE(ReadOnlySpan<byte> span) => ReadInt32(span, false);
        public long ReadInt64BE(ReadOnlySpan<byte> span) => ReadInt64(span, false);
        public ushort ReadUInt16BE(ReadOnlySpan<byte> span) => ReadUInt16(span, false);
        public uint ReadUInt32BE(ReadOnlySpan<byte> span) => ReadUInt32(span, false);
        public ulong ReadUInt64BE(ReadOnlySpan<byte> span) => ReadUInt64(span, false);
        public double ReadDoubleBE(ReadOnlySpan<byte> span) => ReadDouble(span, false);
        public float ReadSingleBE(ReadOnlySpan<byte> span) => ReadSingle(span, false);
#if NET6_0_OR_GREATER
        public Half ReadHalfBE(ReadOnlySpan<byte> span) => ReadHalf(span, false);
#endif

        public short ReadInt16(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadInt16LittleEndian(GetSpan(span, sizeof(short)))
            : BinaryPrimitives.ReadInt16BigEndian(GetSpan(span, sizeof(short)));

        public int ReadInt32(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadInt32LittleEndian(GetSpan(span, sizeof(int)))
            : BinaryPrimitives.ReadInt32BigEndian(GetSpan(span, sizeof(int)));

        public long ReadInt64(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadInt64LittleEndian(GetSpan(span, sizeof(long)))
            : BinaryPrimitives.ReadInt64BigEndian(GetSpan(span, sizeof(long)));

        public ushort ReadUInt16(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadUInt16LittleEndian(GetSpan(span, sizeof(ushort)))
            : BinaryPrimitives.ReadUInt16BigEndian(GetSpan(span, sizeof(ushort)));

        public uint ReadUInt32(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadUInt32LittleEndian(GetSpan(span, sizeof(uint)))
            : BinaryPrimitives.ReadUInt32BigEndian(GetSpan(span, sizeof(uint)));

        public ulong ReadUInt64(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadUInt64LittleEndian(GetSpan(span, sizeof(ulong)))
            : BinaryPrimitives.ReadUInt64BigEndian(GetSpan(span, sizeof(ulong)));

        public double ReadDouble(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitivesMethods.ReadDoubleLittleEndian(GetSpan(span, sizeof(double)))
            : BinaryPrimitivesMethods.ReadDoubleBigEndian(GetSpan(span, sizeof(double)));

        public float ReadSingle(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitivesMethods.ReadSingleLittleEndian(GetSpan(span, sizeof(float)))
            : BinaryPrimitivesMethods.ReadSingleBigEndian(GetSpan(span, sizeof(float)));

#if NET6_0_OR_GREATER
        public Half ReadHalf(ReadOnlySpan<byte> span, bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadHalfLittleEndian(GetSpan(span, Constants.HalfSize))
            : BinaryPrimitives.ReadHalfBigEndian(GetSpan(span, Constants.HalfSize));
#endif
        public ReadOnlySpan<char> ReadString(ReadOnlySpan<byte> span, int charCount, Encoding encoding)
        {
            var maxByteCount = encoding.GetMaxByteCount(charCount);
            var slice = GetSpan(span, Math.Min(_count, maxByteCount));
            var res = EncodingUtils.ConvertFromByteToChar(slice, encoding);
            return res;
        }

        public ReadOnlySpan<char> ReadString(ReadOnlySpan<byte> span, Encoding encoding)
        {
            var slice = span[Position..];
            var index = slice.IndexOf((byte)'\0');
            if (index == -1)
                throw new EndOfStreamException();

            var stringSlice = slice[..index];
            var res = EncodingUtils.ConvertFromByteToChar(stringSlice, encoding);
            return res;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<byte> GetSpan(ReadOnlySpan<byte> span, int size)
        {
            if (Position + size > _count)
                throw new EndOfStreamException();

            var valueSpan = span.Slice(Position, size);
            Position += size;
            return valueSpan;
        }
    }
}
