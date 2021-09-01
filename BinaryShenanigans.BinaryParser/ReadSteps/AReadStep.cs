using System.Text;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal abstract class AReadStep
    {
        public abstract void WriteCode(StringBuilder sb);
    }
}
