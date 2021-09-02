using System;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.Example.Generated
{
    public class AnotherClassParser : IBinaryParser<BinaryShenanigans.Example.AnotherClass>
    {
        public static BinaryShenanigans.Example.AnotherClass ParseStatic(ReadOnlySpan<byte> span)
        {
            var res = new BinaryShenanigans.Example.AnotherClass();
            var reader = new SpanReader(0, span.Length);

            res.UInt32Value = reader.ReadUInt16(span, true);

            return res;
        }

        public BinaryShenanigans.Example.AnotherClass Parse(ReadOnlySpan<byte> span) => ParseStatic(span);
    }
}
