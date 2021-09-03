using System;
using System.Linq;
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

            var lambdaExpression = (LambdaExpression)_conditionalExpression;
            var lambdaBody = lambdaExpression.Body;
            var evaluationString = "false";

            if (lambdaBody is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Type != typeof(bool))
                    throw new NotImplementedException();

                if (methodCallExpression.Object == null)
                    throw new NotImplementedException();

                if (methodCallExpression.Object is not MemberExpression memberExpression)
                    throw new NotImplementedException();

                var memberInfo = memberExpression.Member;
                var methodInfo = methodCallExpression.Method;
                var arguments = methodCallExpression.Arguments
                    .Select(x => x.ToString())
                    .Aggregate((x, y) => $"{x},{y}");
                evaluationString = $"res.{memberInfo.Name}.{methodInfo.Name}({arguments})";
            } else if (lambdaBody is BinaryExpression binaryExpression)
            {
                if (binaryExpression.Type != typeof(bool))
                    throw new NotImplementedException();
                evaluationString = ExpressionUtils.BinaryExpressionToString(binaryExpression, "res");
            }

            codeWriter.WriteNewLine();

            if (WhenTrueBuilder == null && WhenFalseBuilder != null)
            {
                using (codeWriter.UseBrackets($"if (!({evaluationString}))"))
                {
                    WhenFalseBuilder.WriteCode(codeWriter);
                }
            }
            else
            {
                if (WhenTrueBuilder != null)
                {
                    using (codeWriter.UseBrackets($"if ({evaluationString})"))
                    {
                        WhenTrueBuilder.WriteCode(codeWriter);
                    }
                }

                if (WhenFalseBuilder != null)
                {
                    using (codeWriter.UseBrackets("else"))
                    {
                        WhenFalseBuilder?.WriteCode(codeWriter);
                    }
                }
            }
        }
    }
}
