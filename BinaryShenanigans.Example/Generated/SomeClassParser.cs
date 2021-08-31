using System;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.Example.Generated
{
    public class SomeClassParser : IBinaryParser<BinaryShenanigans.Example.SomeClass>
    {
        public BinaryShenanigans.Example.SomeClass Parse(byte[] data)
        {
            var res = new BinaryShenanigans.Example.SomeClass();
            var spanReader = new SpanReader(0, data.Length);
            var span = new ReadOnlySpan<byte>(data, 0, data.Length);
            res.UInt32Property = spanReader.ReadUInt16(span, true);
            res.Int32Field = spanReader.ReadInt32(span, true);
            return res;
        }
    }
}