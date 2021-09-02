using System;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Interfaces
{
    [PublicAPI]
    public interface IBinaryParser<out T>
    {
        T Parse(ReadOnlySpan<byte> span);
    }
}
