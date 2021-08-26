using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace BinaryShenanigans.Writer
{
    [PublicAPI]
    public class BufferedWriter
    {
        private readonly byte[] _buffer;
        private int _pos;
        private readonly int _count;
        private readonly Encoding _encoding;
        
        public bool LittleEndian;
        
        public BufferedWriter(byte[] buffer, int start, int count, Encoding encoding, bool littleEndian = true)
        {
            _buffer = buffer; 
            _pos = start;
            _count = count;
            _encoding = encoding;
            
            LittleEndian = littleEndian;
        }
        
        public void Write(short value) => Write(value, LittleEndian);
        public void Write(ushort value) => Write(value, LittleEndian);
        public void Write(int value) => Write(value, LittleEndian);
        public void Write(uint value) => Write(value, LittleEndian);
        public void Write(long value) => Write(value, LittleEndian);
        public void Write(ulong value) => Write(value, LittleEndian);
        public void Write(double value) => Write(value, LittleEndian);
        public void Write(float value) => Write(value, LittleEndian);
        public void Write(Half value) => Write(value, LittleEndian);
        
        public void WriteLE(short value) => Write(value, true);
        public void WriteLE(ushort value) => Write(value, true);
        public void WriteLE(int value) => Write(value, true);
        public void WriteLE(uint value) => Write(value, true);
        public void WriteLE(long value) => Write(value, true);
        public void WriteLE(ulong value) => Write(value, true);
        public void WriteLE(double value) => Write(value, true);
        public void WriteLE(float value) => Write(value, true);
        public void WriteLE(Half value) => Write(value, true);
        
        public void WriteBE(short value) => Write(value, false);
        public void WriteBE(ushort value) => Write(value, false);
        public void WriteBE(int value) => Write(value, false);
        public void WriteBE(uint value) => Write(value, false);
        public void WriteBE(long value) => Write(value, false);
        public void WriteBE(ulong value) => Write(value, false);
        public void WriteBE(double value) => Write(value, false);
        public void WriteBE(float value) => Write(value, false);
        public void WriteBE(Half value) => Write(value, false);
        
        public void Write(short value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteInt16LittleEndian(GetSpan(sizeof(short)), value);
            else
                BinaryPrimitives.WriteInt16BigEndian(GetSpan(sizeof(short)), value);
        }
        
        public void Write(int value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteInt32LittleEndian(GetSpan(sizeof(int)), value);
            else
                BinaryPrimitives.WriteInt32BigEndian(GetSpan(sizeof(int)), value);
        }
        
        public void Write(long value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteInt64LittleEndian(GetSpan(sizeof(long)), value);
            else
                BinaryPrimitives.WriteInt64BigEndian(GetSpan(sizeof(long)), value);
        }
        
        public void Write(ushort value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteUInt16LittleEndian(GetSpan(sizeof(ushort)), value);
            else
                BinaryPrimitives.WriteUInt16BigEndian(GetSpan(sizeof(ushort)), value);
        }
        
        public void Write(uint value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteUInt32LittleEndian(GetSpan(sizeof(uint)), value);
            else
                BinaryPrimitives.WriteUInt32BigEndian(GetSpan(sizeof(uint)), value);
        }
        
        public void Write(ulong value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteUInt64LittleEndian(GetSpan(sizeof(ulong)), value);
            else
                BinaryPrimitives.WriteUInt64BigEndian(GetSpan(sizeof(ulong)), value);
        }
        
        public void Write(double value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteDoubleLittleEndian(GetSpan(sizeof(double)), value);
            else
                BinaryPrimitives.WriteDoubleBigEndian(GetSpan(sizeof(double)), value);
        }
        
        public void Write(float value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteSingleLittleEndian(GetSpan(sizeof(float)), value);
            else
                BinaryPrimitives.WriteSingleBigEndian(GetSpan(sizeof(float)), value);
        }
        
        public void Write(Half value, bool littleEndian)
        {
            if (littleEndian)
                BinaryPrimitives.WriteHalfLittleEndian(GetSpan(Constants.HalfSize), value);
            else
                BinaryPrimitives.WriteHalfBigEndian(GetSpan(Constants.HalfSize), value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<byte> GetSpan(int size)
        {
            if (_pos + size > _count)
                throw new EndOfStreamException();

            var span = new Span<byte>(_buffer, _pos, size);
            _pos += size;
            return span;
        }
    }
}