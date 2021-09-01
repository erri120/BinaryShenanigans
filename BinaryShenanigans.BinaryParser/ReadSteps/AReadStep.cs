using CodeWriterUtils;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal abstract class AReadStep
    {
        public abstract void WriteCode(CodeWriter codeWriter);
    }
}
