using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser.Interfaces
{
    [PublicAPI]
    public interface IBinaryParser<out T>
    {
        T Parse(byte[] data);
    }
}
