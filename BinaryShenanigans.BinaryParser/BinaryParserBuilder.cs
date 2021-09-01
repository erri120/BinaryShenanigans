using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.ReadSteps;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser
{
    [PublicAPI]
    public static class BinaryParserBuilder
    {
        public static IBinaryParserBuilder<T> Configure<T>() => new BinaryParserBuilder<T>();
    }

    internal abstract class ABinaryParserBuilder
    {
        public abstract Type Type { get; }
        public abstract List<AReadStep> Steps { get; }

        public void WriteCode(StringBuilder sb)
        {
            foreach (var step in Steps)
            {
                step.WriteCode(sb);
            }
        }
    }

    [PublicAPI]
    internal partial class BinaryParserBuilder<T> : ABinaryParserBuilder, IBinaryParserBuilder<T>, IBinaryParserBuilderWithConditionBranch<T>
    {
        public override Type Type => typeof(T);

        private readonly List<AReadStep> _steps = new();
        public override List<AReadStep> Steps => _steps;

        private readonly BinaryParserBuilder<T>? _parent;

        public BinaryParserBuilder() { }

        public BinaryParserBuilder(BinaryParserBuilder<T> parent)
        {
            _parent = parent;
        }

        public IBinaryParserBuilder<T> SkipBytes(ulong count)
        {
            _steps.Add(new SkipBytesStep(count));
            return this;
        }

        public IBinaryParserBuilderWithConditionBranch<T> WithCondition(Expression<Func<T, bool>> conditionExpression, Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> conditionMetExpression)
        {
            // TODO:
            return this;
        }
    }
}
