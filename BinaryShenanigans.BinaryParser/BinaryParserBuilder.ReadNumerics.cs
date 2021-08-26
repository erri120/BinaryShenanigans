using System;
using System.Linq.Expressions;
using BinaryShenanigans.BinaryParser.Interfaces;

namespace BinaryShenanigans.BinaryParser
{
    public partial class BinaryParserBuilder<T>
    {
        public IBinaryParserBuilder<T> ReadInt16(Expression<Func<T, short>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadUInt16(Expression<Func<T, ushort>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadInt32(Expression<Func<T, int>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadUInt32(Expression<Func<T, uint>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadInt64(Expression<Func<T, long>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadUInt64(Expression<Func<T, ulong>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadDouble(Expression<Func<T, double>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadSingle(Expression<Func<T, float>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }

        public IBinaryParserBuilder<T> ReadHalf(Expression<Func<T, Half>> expression, bool littleEndian = true)
        {
            throw new NotImplementedException();
        }
    }
}