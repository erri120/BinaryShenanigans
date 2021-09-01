using System;
using System.Linq.Expressions;
using System.Text;

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

        public override void WriteCode(StringBuilder sb)
        {
            if (_conditionalExpression.NodeType != ExpressionType.Lambda)
                throw new NotImplementedException();

            // TODO: find a better way to extract the condition
            var lambdaExpression = (LambdaExpression)_conditionalExpression;
            var bodyString = lambdaExpression.Body.ToString().Replace(lambdaExpression.Parameters[0].Name!, "res");

            sb.Append(@$"

            if ({bodyString})
            {{");

            WhenTrueBuilder?.WriteCode(sb);

            sb.Append(@"
            }
            else
            {");

            WhenFalseBuilder?.WriteCode(sb);

            sb.Append(@"
            }
");
        }
    }
}
