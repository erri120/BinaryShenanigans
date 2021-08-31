using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.Steps;
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
        public abstract List<AStep> Steps { get; }
    }

    [PublicAPI]
    internal partial class BinaryParserBuilder<T> : ABinaryParserBuilder, IBinaryParserBuilderIfBranch<T>, IBinaryParserBuilderWithConditionBranch<T>, IBinaryParserBuilderIfBranchWhenTruePath<T>, IBinaryParserBuilderIfBranchWhenFalsePath<T>
    {
        public override Type Type => typeof(T);

        private readonly List<AStep> _steps = new();
        public override List<AStep> Steps => _steps;

        public IBinaryParserBuilderIfBranch<T> If(Expression<Func<T, bool>> expression)
        {
            // TODO:
            return this;
        }

        public IBinaryParserBuilderWithConditionBranch<T> WithCondition(Expression<Func<T, bool>> conditionExpression, Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> conditionMetExpression)
        {
            // TODO:
            return this;
        }

        public IBinaryParserBuilder<T> SkipBytes(ulong count)
        {
            // TODO:
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T> WhenTrue()
        {
            // TODO:
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T> WhenFalse()
        {
            // TODO:
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenTruePath<T> WhenTrue(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression)
        {
            // TODO:
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenFalsePath<T> WhenFalse(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression)
        {
            // TODO:
            return this;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T>.WhenFalse(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression)
        {
            // TODO:
            return this;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T>.WhenTrue(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression)
        {
            // TODO:
            return this;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenTruePath<T>.WhenFalse()
        {
            // TODO:
            return this;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenFalsePath<T>.WhenTrue()
        {
            // TODO:
            return this;
        }
    }
}
