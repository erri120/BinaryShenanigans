using BinaryShenanigans.BinaryParser;
using BinaryShenanigans.BinaryParser.Interfaces;

namespace BinaryShenanigans.Example
{
    public class SomeClass
    {
        public uint UInt32Property { get; set; }
        public int Int32Field;
    }

    public class SomeClassConfiguration : IBinaryParserConfiguration<SomeClass>
    {
        public IBinaryParserBuilder<SomeClass> Configure()
        {
            return BinaryParserBuilder.Configure<SomeClass>()
                .ReadUInt32(x => x.UInt32Property)
                .ReadInt32(x => x.Int32Field);
        }
    }
}
