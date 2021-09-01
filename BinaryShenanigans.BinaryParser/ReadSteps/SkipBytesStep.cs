using System.Text;
using BinaryShenanigans.Reader;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal class SkipBytesStep : AReadStep
    {
        private readonly ulong _count;

        public SkipBytesStep(ulong count)
        {
            _count = count;
        }

        public override void WriteCode(StringBuilder sb)
        {
            sb.Append(@$"
            reader.{nameof(SpanReader.SkipBytes)}({_count});");
        }
    }
}
