using System.Collections.Generic;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Gen.Generator
{
    [PublicAPI]
    public interface IBinaryParserInMemoryGenerator : IBinaryParserGenerator
    {
        Dictionary<string, string> GeneratedOutput { get; }
    }
}
