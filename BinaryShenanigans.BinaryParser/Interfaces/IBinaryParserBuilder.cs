using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Interfaces
{
    [PublicAPI]
    public interface IBinaryParserBuilder<T>
    {
        #region Logic

        IBinaryParserBuilderIfBranch<T> If(Expression<Func<T, bool>> expression);

        IBinaryParserBuilderWithConditionBranch<T> WithCondition(Expression<Func<T, bool>> conditionExpression,
            Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> conditionMetExpression);

        #endregion

        #region Skips

        IBinaryParserBuilder<T> SkipBytes(ulong count);

        #endregion

        #region Read Functions

        #region Numerics

        IBinaryParserBuilder<T> ReadInt16(Expression<Func<T, short>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadUInt16(Expression<Func<T, ushort>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadInt32(Expression<Func<T, int>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadUInt32(Expression<Func<T, uint>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadInt64(Expression<Func<T, long>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadUInt64(Expression<Func<T, ulong>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadDouble(Expression<Func<T, double>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadSingle(Expression<Func<T, float>> expression, bool littleEndian = true);
        IBinaryParserBuilder<T> ReadHalf(Expression<Func<T, Half>> expression, bool littleEndian = true);

        #endregion

        #endregion
    }
}
