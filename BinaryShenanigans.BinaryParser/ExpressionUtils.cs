using System.Linq.Expressions;
using System.Reflection;

namespace BinaryShenanigans.BinaryParser
{
    internal static class ExpressionUtils
    {
        public static MemberInfo? GetMemberInfoFromExpression(Expression expression)
        {
            if (expression.NodeType != ExpressionType.Lambda) return null;

            var lambdaExpression = (LambdaExpression)expression;
            var lambdaBody = lambdaExpression.Body;

            if (lambdaBody.NodeType != ExpressionType.MemberAccess) return null;

            var memberExpression = (MemberExpression)lambdaBody;
            return memberExpression.Member;
        }
    }
}
