using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Gen.Generator
{
    [PublicAPI]
    public interface IBinaryParserGenerator
    {
        bool Run();
    }
}
