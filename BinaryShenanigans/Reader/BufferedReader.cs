using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BinaryShenanigans.Reader
{
    [PublicAPI]
    public class BufferedReader
    {
        private readonly byte[] _buffer;
        private readonly int _count;

        public bool LittleEndian;
        public int Position { get; private set; }

        public BufferedReader(byte[] buffer, bool littleEndian = true) :
            this(buffer, 0, buffer.Length, littleEndian) { }

        public BufferedReader(byte[] buffer, int start, int count, bool littleEndian = true)
        {
            _buffer = buffer;
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

        public short ReadInt16() => ReadInt16(LittleEndian);
        public int ReadInt32() => ReadInt32(LittleEndian);
        public long ReadInt64() => ReadInt64(LittleEndian);
        public ushort ReadUInt16() => ReadUInt16(LittleEndian);
        public uint ReadUInt32() => ReadUInt32(LittleEndian);
        public ulong ReadUInt64() => ReadUInt64(LittleEndian);
        public double ReadDouble() => ReadDouble(LittleEndian);
        public float ReadSingle() => ReadSingle(LittleEndian);
        public Half ReadHalf() => ReadHalf(LittleEndian);

        public short ReadInt16LE() => ReadInt16(true);
        public int ReadInt32LE() => ReadInt32(true);
        public long ReadInt64LE() => ReadInt64(true);
        public ushort ReadUInt16LE() => ReadUInt16(true);
        public uint ReadUInt32LE() => ReadUInt32(true);
        public ulong ReadUInt64LE() => ReadUInt64(true);
        public double ReadDoubleLE() => ReadDouble(true);
        public float ReadSingleLE() => ReadSingle(true);
        public Half ReadHalfLE() => ReadHalf(true);

        public short ReadInt16BE() => ReadInt16(false);
        public int ReadInt32BE() => ReadInt32(false);
        public long ReadInt64BE() => ReadInt64(false);
        public ushort ReadUInt16BE() => ReadUInt16(false);
        public uint ReadUInt32BE() => ReadUInt32(false);
        public ulong ReadUInt64BE() => ReadUInt64(false);
        public double ReadDoubleBE() => ReadDouble(false);
        public float ReadSingleBE() => ReadSingle(false);
        public Half ReadHalfBE() => ReadHalf(false);

        public short ReadInt16(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadInt16LittleEndian(GetSpan(sizeof(short)))
            : BinaryPrimitives.ReadInt16BigEndian(GetSpan(sizeof(short)));

        public int ReadInt32(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadInt32LittleEndian(GetSpan(sizeof(int)))
            : BinaryPrimitives.ReadInt32BigEndian(GetSpan(sizeof(int)));

        public long ReadInt64(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadInt64LittleEndian(GetSpan(sizeof(long)))
            : BinaryPrimitives.ReadInt64BigEndian(GetSpan(sizeof(long)));

        public ushort ReadUInt16(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadUInt16LittleEndian(GetSpan(sizeof(ushort)))
            : BinaryPrimitives.ReadUInt16BigEndian(GetSpan(sizeof(ushort)));

        public uint ReadUInt32(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadUInt32LittleEndian(GetSpan(sizeof(uint)))
            : BinaryPrimitives.ReadUInt32BigEndian(GetSpan(sizeof(uint)));

        public ulong ReadUInt64(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadUInt64LittleEndian(GetSpan(sizeof(ulong)))
            : BinaryPrimitives.ReadUInt64BigEndian(GetSpan(sizeof(ulong)));

        public double ReadDouble(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadDoubleLittleEndian(GetSpan(sizeof(double)))
            : BinaryPrimitives.ReadDoubleBigEndian(GetSpan(sizeof(double)));

        public float ReadSingle(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadSingleLittleEndian(GetSpan(sizeof(float)))
            : BinaryPrimitives.ReadSingleBigEndian(GetSpan(sizeof(float)));

        public Half ReadHalf(bool littleEndian) => littleEndian
            ? BinaryPrimitives.ReadHalfLittleEndian(GetSpan(Constants.HalfSize))
            : BinaryPrimitives.ReadHalfBigEndian(GetSpan(Constants.HalfSize));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlySpan<byte> GetSpan(int size)
        {
            if (Position + size > _count)
                throw new EndOfStreamException();

            var span = new ReadOnlySpan<byte>(_buffer, Position, size);
            Position += size;
            return span;
        }
    }
}
