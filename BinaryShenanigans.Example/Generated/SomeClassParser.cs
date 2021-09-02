using System;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.Example.Generated
{
    public class SomeClassParser : IBinaryParser<BinaryShenanigans.Example.SomeClass>
    {
        public static BinaryShenanigans.Example.SomeClass ParseStatic(ReadOnlySpan<byte> span)
        {
            var res = new BinaryShenanigans.Example.SomeClass();
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

            if (res.Int32Value.Equals(1377))
            {
                res.Int64Value = reader.ReadInt64(span, true);
            }
            else
            {
                res.UInt64Value = reader.ReadUInt16(span, true);
            }
            res.AnotherClass = AnotherClassParser.ParseStatic(span.Slice(reader.Position));

            return res;
        }

        public BinaryShenanigans.Example.SomeClass Parse(ReadOnlySpan<byte> span) => ParseStatic(span);
    }
}
