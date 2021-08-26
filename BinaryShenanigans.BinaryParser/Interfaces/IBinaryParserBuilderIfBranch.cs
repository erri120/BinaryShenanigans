using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Interfaces
{
    [PublicAPI]
    public interface IBinaryParserBuilderIfBranch<T>
    {
        IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T> WhenTrue();
        IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T> WhenFalse();
        IBinaryParserBuilderIfBranchWhenTruePath<T> WhenTrue(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression);
        IBinaryParserBuilderIfBranchWhenFalsePath<T> WhenFalse(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression);
    }

    [PublicAPI]
    public interface IBinaryParserBuilderIfBranchWhenTruePath<T> : IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T>
    {
        IBinaryParserBuilder<T> WhenFalse();
    }

    [PublicAPI]
    public interface IBinaryParserBuilderIfBranchWhenFalsePath<T> : IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T>
    {
        IBinaryParserBuilder<T> WhenTrue();
    }

    [PublicAPI]
    public interface IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T>
    {
        IBinaryParserBuilder<T> WhenFalse(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression);
    }
    
    [PublicAPI]
    public interface IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T>
    {
        IBinaryParserBuilder<T> WhenTrue(Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> expression);
    }
}