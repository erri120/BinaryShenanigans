using System;
using System.Buffers.Binary;
#if NETSTANDARD2_1
using System.Runtime.InteropServices;
#endif

namespace BinaryShenanigans
{
    internal static class BinaryPrimitivesMethods
    {
        public static double ReadDoubleBigEndian(ReadOnlySpan<byte> source)
        {
#if NETSTANDARD2_1
            return BitConverter.IsLittleEndian ?
                BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(source))) :
                MemoryMarshal.Read<double>(source);
#else
            return BinaryPrimitives.ReadDoubleBigEndian(source);
#endif
        }

        public static double ReadDoubleLittleEndian(ReadOnlySpan<byte> source)
        {
#if NETSTANDARD2_1
            return !BitConverter.IsLittleEndian ?
                BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<long>(source))) :
                MemoryMarshal.Read<double>(source);
#else
            return BinaryPrimitives.ReadDoubleLittleEndian(source);
#endif
        }

        public static float ReadSingleBigEndian(ReadOnlySpan<byte> source)
        {
#if NETSTANDARD2_1
            return BitConverter.IsLittleEndian ?
                BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<int>(source))) :
                MemoryMarshal.Read<float>(source);
#else
            return BinaryPrimitives.ReadSingleBigEndian(source);
#endif
        }

        public static float ReadSingleLittleEndian(ReadOnlySpan<byte> source)
        {
#if NETSTANDARD2_1
            return !BitConverter.IsLittleEndian ?
                BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(MemoryMarshal.Read<int>(source))) :
                MemoryMarshal.Read<float>(source);
#else
            return BinaryPrimitives.ReadSingleLittleEndian(source);
#endif
        }

        public static void WriteDoubleBigEndian(Span<byte> destination, double value)
        {
#if NETSTANDARD2_1
            if (BitConverter.IsLittleEndian)
            {
                var tmp = BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value));
                MemoryMarshal.Write(destination, ref tmp);
            }
            else
            {
                MemoryMarshal.Write(destination, ref value);
            }
#else
            BinaryPrimitives.WriteDoubleBigEndian(destination, value);
#endif
        }

        public static void WriteDoubleLittleEndian(Span<byte> destination, double value)
        {
#if NETSTANDARD2_1
            if (!BitConverter.IsLittleEndian)
            {
                var tmp = BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value));
                MemoryMarshal.Write(destination, ref tmp);
            }
            else
            {
                MemoryMarshal.Write(destination, ref value);
            }
#else
            BinaryPrimitives.WriteDoubleLittleEndian(destination, value);
#endif
        }

        public static void WriteSingleBigEndian(Span<byte> destination, float value)
        {
#if NETSTANDARD2_1
            if (BitConverter.IsLittleEndian)
            {
                var tmp = BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value));
                MemoryMarshal.Write(destination, ref tmp);
            }
            else
            {
                MemoryMarshal.Write(destination, ref value);
            }
#else
            BinaryPrimitives.WriteSingleBigEndian(destination, value);
#endif
        }

        public static void WriteSingleLittleEndian(Span<byte> destination, float value)
        {
#if NETSTANDARD2_1
            if (!BitConverter.IsLittleEndian)
            {
                var tmp = BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value));
                MemoryMarshal.Write(destination, ref tmp);
            }
            else
            {
                MemoryMarshal.Write(destination, ref value);
            }
#else
            BinaryPrimitives.WriteSingleLittleEndian(destination, value);
#endif
        }
    }
}
