using System.Reflection;
using CodeWriterUtils;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal class CustomLogicStep : AReadStep
    {
        private readonly MethodInfo _methodInfo;

        public CustomLogicStep(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }

        public override void WriteCode(CodeWriter codeWriter)
        {
            codeWriter.WriteLine($"{_methodInfo.DeclaringType!.FullName}.{_methodInfo.Name}(res, reader, span);");
        }
    }
}
