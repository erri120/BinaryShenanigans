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
        IBinaryParserBuilderIfBranchWhenTruePath<T> WhenTrue(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func);
        IBinaryParserBuilderIfBranchWhenFalsePath<T> WhenFalse(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func);
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
        IBinaryParserBuilder<T> WhenFalse(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func);
    }

    [PublicAPI]
    public interface IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T>
    {
        IBinaryParserBuilder<T> WhenTrue(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func);
    }
}
