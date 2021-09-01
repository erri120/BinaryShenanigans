using System;
using System.Linq.Expressions;
using System.Text;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal class ReadNumbersStep : AReadStep
    {
        private readonly Expression _expression;
        private readonly NumericType _numericType;
        private readonly bool _littleEndian;

        public ReadNumbersStep(Expression expression, NumericType numericType, bool littleEndian)
        {
            _expression = expression;
            _numericType = numericType;
            _littleEndian = littleEndian;
        }

        public override void WriteCode(StringBuilder sb)
        {
            var memberInfo = ExpressionUtils.GetMemberInfoFromExpression(_expression);
            if (memberInfo == null)
                throw new NotImplementedException();

            var memberName = memberInfo.Name;
            var readerFunction = _numericType switch
            {
                NumericType.Int16 => nameof(SpanReader.ReadInt16),
                NumericType.UInt16 => nameof(SpanReader.ReadUInt16),
                NumericType.Int32 => nameof(SpanReader.ReadInt32),
                NumericType.UInt32 => nameof(SpanReader.ReadUInt16),
                NumericType.Int64 => nameof(SpanReader.ReadInt64),
                NumericType.UInt64 => nameof(SpanReader.ReadUInt16),
                NumericType.Single => nameof(SpanReader.ReadSingle),
                NumericType.Double => nameof(SpanReader.ReadDouble),
                NumericType.Half => nameof(SpanReader.ReadHalf),
                _ => throw new ArgumentOutOfRangeException()
            };

            sb.Append($@"
            res.{memberName} = reader.{readerFunction}(span, {(_littleEndian ? "true" : "false")});");
        }
    }

    internal enum NumericType
    {
        Int16,
        UInt16,
        Int32,
        UInt32,
        Int64,
        UInt64,
        Single,
        Double,
        Half
    }
}
