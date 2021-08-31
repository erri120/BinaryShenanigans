using System;
using System.Linq.Expressions;

namespace BinaryShenanigans.BinaryParser.Steps
{
    internal class ReadNumericsStep : AStep
    {
        public override StepType StepType => StepType.ReadNumerics;

        public readonly Expression Expression;
        public readonly NumericType NumericType;
        public readonly bool LittleEndian;

        public ReadNumericsStep(Expression expression, NumericType numericType, bool littleEndian)
        {
            Expression = expression;
            NumericType = numericType;
            LittleEndian = littleEndian;
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
