using System;
using System.Buffers.Binary;
using JetBrains.Annotations;

namespace BinaryShenanigans.Writer
{
    [PublicAPI]
    public static class BinaryWriterExtensions
    {
        public static void WriteInt16BE(this System.IO.BinaryWriter bw, short value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(short)];
            BinaryPrimitives.WriteInt16BigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteInt32BE(this System.IO.BinaryWriter bw, int value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteInt64BE(this System.IO.BinaryWriter bw, long value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteUInt16BE(this System.IO.BinaryWriter bw, ushort value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteUInt32BE(this System.IO.BinaryWriter bw, uint value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteUInt64BE(this System.IO.BinaryWriter bw, ulong value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteDoubleBE(this System.IO.BinaryWriter bw, double value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(double)];
            BinaryPrimitivesMethods.WriteDoubleBigEndian(buffer, value);
            bw.Write(buffer);
        }

        public static void WriteSingleBE(this System.IO.BinaryWriter bw, float value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(float)];
            BinaryPrimitivesMethods.WriteSingleBigEndian(buffer, value);
            bw.Write(buffer);
        }

#if NET6_0_OR_GREATER
        public static void WriteHalfBE(this System.IO.BinaryWriter bw, Half value)
        {
            Span<byte> buffer = stackalloc byte[sizeof(ushort) /* = sizeof(Half) */];
            BinaryPrimitives.WriteHalfBigEndian(buffer, value);
            bw.Write(buffer);
        }
#endif

        public static void WriteBE(this System.IO.BinaryWriter bw, short value) => WriteInt16BE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, ushort value) => WriteUInt16BE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, int value) => WriteInt32BE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, uint value) => WriteUInt32BE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, long value) => WriteInt64BE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, ulong value) => WriteUInt64BE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, double value) => WriteDoubleBE(bw, value);
        public static void WriteBE(this System.IO.BinaryWriter bw, float value) => WriteSingleBE(bw, value);
#if NET6_0_OR_GREATER
        public static void WriteBE(this System.IO.BinaryWriter bw, Half value) => WriteHalfBE(bw, value);
#endif
    }
}
