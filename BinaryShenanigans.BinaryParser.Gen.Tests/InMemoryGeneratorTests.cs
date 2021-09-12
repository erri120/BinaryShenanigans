using System;
using System.Reflection;
using BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder;
using BinaryShenanigans.BinaryParser.Interfaces;
using Xunit;

namespace BinaryShenanigans.BinaryParser.Gen.Tests
{
    public class InMemoryGeneratorTests
    {
        [Fact]
        public void TestGeneration()
        {
            const string expectedOutput = @"using System;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.BinaryParser.Gen.Tests.Generated
{
    public class SomeClassParser : IBinaryParser<BinaryShenanigans.BinaryParser.Gen.Tests.SomeClass>
    {
        public static BinaryShenanigans.BinaryParser.Gen.Tests.SomeClass ParseStatic(ReadOnlySpan<byte> span)
        {
            var res = new BinaryShenanigans.BinaryParser.Gen.Tests.SomeClass();
            var reader = new SpanReader(0, span.Length);

            res.Int16Value = reader.ReadInt16(span, true);
            res.UInt16Value = reader.ReadUInt16(span, true);
            res.Int32Value = reader.ReadInt32(span, true);
            res.UInt32Value = reader.ReadUInt16(span, true);
            res.Int64Value = reader.ReadInt64(span, true);
            res.UInt64Value = reader.ReadUInt16(span, true);
            res.DoubleValue = reader.ReadDouble(span, true);
            res.SingleValue = reader.ReadSingle(span, true);
            res.HalfValue = reader.ReadHalf(span, true);
            reader.SkipBytes(8);

            if (!((res.Int16Value == 1) && (res.Int32Value == 2)))
            {
                res.HalfValue = reader.ReadHalf(span, true);
            }

            if (!((res.Int16Value == 1) || (res.Int32Value == 2)))
            {
                res.HalfValue = reader.ReadHalf(span, true);
            }

            if (res.Int32Value.Equals(1377))
            {
                res.Int64Value = reader.ReadInt64(span, true);
            }
            else
            {
                res.UInt64Value = reader.ReadUInt16(span, true);
            }

            if (res.Int32Value == 1337)
            {
                res.Int16Value = reader.ReadInt16(span, true);
            }

            if (!(res.Int16Value == res.Int32Value))
            {
                res.HalfValue = reader.ReadHalf(span, true);
            }

            return res;
        }

        public BinaryShenanigans.BinaryParser.Gen.Tests.SomeClass Parse(ReadOnlySpan<byte> span) => ParseStatic(span);
    }
}
";

            var generator = BinaryParserInMemoryGeneratorBuilder
                .CreateBuilder()
                .AddType((TypeInfo)typeof(SomeClassConfiguration))
                .Build();

            Assert.True(generator.Run());
            Assert.Equal(expectedOutput, generator.GeneratedOutput["SomeClassParser.cs"].Replace("\n", "\r\n"));
        }
    }

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
                .If(x => x.Int16Value == 1 && x.Int32Value == 2)
                    .WhenTrue()
                    .WhenFalse(b => b.ReadHalf(x => x.HalfValue))
                .If(x => x.Int16Value == 1 || x.Int32Value == 2)
                    .WhenTrue()
                    .WhenFalse(b => b.ReadHalf(x => x.HalfValue))
                .If(x => x.Int32Value.Equals(1377))
                    .WhenTrue(b => b.ReadInt64(x => x.Int64Value))
                    .WhenFalse(b => b.ReadUInt64(x => x.UInt64Value))
                .If(x => x.Int32Value == 1337)
                    .WhenTrue(b => b.ReadInt16(x => x.Int16Value))
                    .WhenFalse()
                .If(x => x.Int16Value == x.Int32Value)
                    .WhenTrue()
                    .WhenFalse(b => b.ReadHalf(x => x.HalfValue));
        }
    }
}
