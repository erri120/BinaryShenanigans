using System;
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

        public static string BinaryExpressionToString(BinaryExpression binaryExpression, string instanceName)
        {
            var left = RemoveUnaryExpression(binaryExpression.Left);
            var right = RemoveUnaryExpression(binaryExpression.Right);

            var leftString = BinaryExpressionSideToString(left, instanceName);
            var rightString = BinaryExpressionSideToString(right, instanceName);

            var operand = binaryExpression.NodeType switch
            {
                ExpressionType.AndAlso => "&&",
                ExpressionType.OrElse => "||",
                _ => "=="
            };

            return $"{leftString} {operand} {rightString}";
        }

        public static Expression RemoveUnaryExpression(Expression expression)
        {
            if (expression is not UnaryExpression unaryExpression)
                return expression;

            if (unaryExpression.NodeType != ExpressionType.Convert)
                return expression;

            return unaryExpression.Operand;
        }

        private static string BinaryExpressionSideToString(Expression expression, string instanceName)
        {
            if (expression is BinaryExpression binaryExpression)
                return $"({BinaryExpressionToString(binaryExpression, instanceName)})";

            if (expression is ConstantExpression constantExpression)
                return $"{constantExpression.Value}";

            if (expression is MemberExpression memberExpression)
            {
                var memberInfo = memberExpression.Member;
                var memberName = memberInfo.Name;

                return $"{instanceName}.{memberName}";
            }

            throw new NotImplementedException();
        }
    }
}
