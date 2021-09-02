using System;
using BinaryShenanigans.BinaryParser;
using BinaryShenanigans.BinaryParser.Interfaces;

namespace BinaryShenanigans.Example
{
    public class SomeClass
    {
        public short Int16Value { get; set; }
        public ushort UInt16Value { get; set; }
        public int Int32Value { get; set; }
        public uint UInt32Value { get; set; }
        public long Int64Value { get; set; }
        public ulong UInt64Value { get; set; }
        public double DoubleValue { get; set; }
        public float SingleValue { get; set; }
        public Half HalfValue { get; set; }

        public AnotherClass? AnotherClass { get; set; }
    }

    public class AnotherClass
    {
        public uint UInt32Value { get; set; }
    }

    public class SomeClassConfiguration : IBinaryParserConfiguration<SomeClass>
    {
        public IBinaryParserBuilder<SomeClass> Configure()
        {
            return BinaryParserBuilder.Configure<SomeClass>()
                .ReadInt16(x => x.Int16Value)
                .ReadUInt16(x => x.UInt16Value)
                .ReadInt32(x => x.Int32Value)
                .ReadUInt32(x => x.UInt32Value)
                .ReadInt64(x => x.Int64Value)
                .ReadUInt64(x => x.UInt64Value)
                .ReadDouble(x => x.DoubleValue)
                .ReadSingle(x => x.SingleValue)
                .ReadHalf(x => x.HalfValue)
                .SkipBytes(8)
                .If(x => x.Int32Value.Equals(1377))
                    .WhenTrue(b => b.ReadInt64(x => x.Int64Value))
                    .WhenFalse(b => b.ReadUInt64(x => x.UInt64Value))
                .ReadOther(x => x.AnotherClass, typeof(AnotherClassConfiguration));
        }
    }

    public class AnotherClassConfiguration : IBinaryParserConfiguration<AnotherClass>
    {
        public IBinaryParserBuilder<AnotherClass> Configure()
        {
            return BinaryParserBuilder.Configure<AnotherClass>()
                .ReadUInt32(x => x.UInt32Value);
        }
    }
}
