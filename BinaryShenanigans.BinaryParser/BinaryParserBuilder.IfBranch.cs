using System;
using System.Linq.Expressions;
using BinaryShenanigans.BinaryParser.Interfaces;

namespace BinaryShenanigans.BinaryParser
{
    internal partial class BinaryParserBuilder<T>
    {
        public IBinaryParserBuilderIfBranch<T> If(Expression<Func<T, bool>> expression)
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
