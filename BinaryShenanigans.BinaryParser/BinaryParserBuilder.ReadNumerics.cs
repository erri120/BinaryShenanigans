using System;
using System.Linq.Expressions;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.ReadSteps;

namespace BinaryShenanigans.BinaryParser
{
    internal partial class BinaryParserBuilder<T>
    {
        public IBinaryParserBuilder<T> ReadInt16(Expression<Func<T, short>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.Int16, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadUInt16(Expression<Func<T, ushort>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.UInt16, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadInt32(Expression<Func<T, int>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.Int32, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadUInt32(Expression<Func<T, uint>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.UInt32, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadInt64(Expression<Func<T, long>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.Int64, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadUInt64(Expression<Func<T, ulong>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.UInt64, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadDouble(Expression<Func<T, double>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.Double, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadSingle(Expression<Func<T, float>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.Single, littleEndian));
            return this;
        }

        public IBinaryParserBuilder<T> ReadHalf(Expression<Func<T, Half>> expression, bool littleEndian = true)
        {
            _steps.Add(new ReadNumbersStep(expression, NumericType.Half, littleEndian));
            return this;
        }
    }
}
