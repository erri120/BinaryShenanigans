using System.Text;
using BinaryShenanigans.Reader;
using CodeWriterUtils;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal class SkipBytesStep : AReadStep
    {
        private readonly ulong _count;

        public SkipBytesStep(ulong count)
        {
            _count = count;
        }

        public override void WriteCode(CodeWriter codeWriter)
        {
            codeWriter.WriteLine($"reader.{nameof(SpanReader.SkipBytes)}({_count});");
        }
    }
}
