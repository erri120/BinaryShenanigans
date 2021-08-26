using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Interfaces
{
    [PublicAPI]
    public interface IBinaryParserBuilderWithConditionBranch<T> : IBinaryParserBuilder<T> { }
}