using System;
using System.Linq.Expressions;
using System.Text;
using CodeWriterUtils;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal class IfBranchStep : AReadStep
    {
        private readonly Expression _conditionalExpression;

        public ABinaryParserBuilder? WhenTrueBuilder { get; set; }
        public ABinaryParserBuilder? WhenFalseBuilder { get; set; }

        public IfBranchStep(Expression conditionalExpression)
        {
            _conditionalExpression = conditionalExpression;
        }

        public override void WriteCode(CodeWriter codeWriter)
        {
            if (_conditionalExpression.NodeType != ExpressionType.Lambda)
                throw new NotImplementedException();

            // TODO: find a better way to extract the condition
            var lambdaExpression = (LambdaExpression)_conditionalExpression;
            var bodyString = lambdaExpression.Body.ToString().Replace(lambdaExpression.Parameters[0].Name!, "res");

            codeWriter.WriteNewLine();
            using (codeWriter.UseBrackets($"if ({bodyString})"))
            {
                WhenTrueBuilder?.WriteCode(codeWriter);
            }

            using (codeWriter.UseBrackets("else"))
            {
                WhenFalseBuilder?.WriteCode(codeWriter);
            }
        }
    }
}
