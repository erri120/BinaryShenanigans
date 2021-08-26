using System;
using System.Buffers.Binary;
using System.IO;
using JetBrains.Annotations;

namespace BinaryShenanigans.Reader
{
    [PublicAPI]
    public static class BinaryReaderExtensions
    {
        public static short ReadInt16BE(this BinaryReader br) => BinaryPrimitives.ReadInt16BigEndian(ReadBytes(br, sizeof(short)));
        public static int ReadInt32BE(this BinaryReader br) => BinaryPrimitives.ReadInt32BigEndian(ReadBytes(br, sizeof(int)));
        public static long ReadInt64BE(this BinaryReader br) => BinaryPrimitives.ReadInt64BigEndian(ReadBytes(br, sizeof(long)));
        public static ushort ReadUInt16BE(this BinaryReader br) => BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(br, sizeof(ushort)));
        public static uint ReadUInt32BE(this BinaryReader br) => BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(br, sizeof(uint)));
        public static ulong ReadUInt64BE(this BinaryReader br) => BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(br, sizeof(ulong)));
        public static double ReadDoubleBE(this BinaryReader br) => BinaryPrimitivesMethods.ReadDoubleBigEndian(ReadBytes(br, sizeof(double)));
        public static float ReadSingleBE(this BinaryReader br) => BinaryPrimitivesMethods.ReadSingleBigEndian(ReadBytes(br, sizeof(float)));

#if NET6_0_OR_GREATER
        public static Half ReadHalfBE(this BinaryReader br) => BinaryPrimitives.ReadHalfBigEndian(ReadBytes(br, sizeof(ushort)));
#endif

        private static ReadOnlySpan<byte> ReadBytes(BinaryReader br, int size)
        {
            // better performance and memory usage if we access the MemoryStream directly instead of using the Stream
            // functions
            if (br.BaseStream is MemoryStream ms)
            {
                // BinaryReader uses MemoryStream.InternalReadSpan which completely bypasses the exposed buffer
                // functions of the MemoryStream and doesn't give a damn about whether it is exposed or not.
                if (ms.TryGetBuffer(out var segment))
                {
                    if (size > segment.Count)
                        throw new EndOfStreamException();

                    var res = new ReadOnlySpan<byte>(segment.Array, segment.Offset, Math.Min(segment.Count, size));

                    // Seek is faster than updating Position
                    ms.Seek(size, SeekOrigin.Current);

                    return res;
                }
            }

            var buffer = new byte[size];
            var count = br.Read(new Span<byte>(buffer, 0, size));
            if (count != size)
                throw new EndOfStreamException();
            return new ReadOnlySpan<byte>(buffer, 0, size);
        }
    }
}
