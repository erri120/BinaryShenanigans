using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BinaryShenanigans.BinaryParser.Interfaces;

namespace BinaryShenanigans.BinaryParser
{
    public partial class BinaryParserBuilder<T>
    {
        /*private IBinaryParserBuilder<T> AddToList(LambdaExpression expression, bool littleEndian)
        {
            var memberInfo = GetMemberInfoFromExpression(expression);
            if (memberInfo == null)
                throw new NotImplementedException();
            
            _parsingSettings.Add(new MemberParsingSettings(memberInfo, littleEndian));
            return this;
        }*/
        
        private static MemberInfo? GetMemberInfoFromExpression(LambdaExpression memberAccessExpression)
        {
            if (memberAccessExpression.Body.NodeType != ExpressionType.MemberAccess)
                throw new NotImplementedException();

            var parameterExpression = memberAccessExpression.Parameters[0];

            var memberInfos = new List<MemberInfo>();
            MemberExpression? memberExpression;

            do
            {
                memberExpression = RemoveTypeAs(RemoveConvert(memberAccessExpression)) as MemberExpression;

                if (memberExpression?.Member is not MemberInfo memberInfo)
                    return null;

                memberInfos.Insert(0, memberInfo);
            } while (RemoveTypeAs(RemoveConvert(memberExpression.Expression!)) != parameterExpression);

            return memberInfos.Count == 1 ? memberInfos.First() : null;
        }

        private static Expression RemoveTypeAs(Expression expression)
        {
            while (expression.NodeType == ExpressionType.TypeAs)
            {
                expression = ((UnaryExpression)RemoveConvert(expression)).Operand;
            }

            return expression;
        }
        
        private static Expression RemoveConvert(Expression expression)
        {
            while (true)
            {
                if (expression is not UnaryExpression unaryExpression || expression.NodeType is not (ExpressionType.Convert or ExpressionType.ConvertChecked))
                    return expression;
                expression = unaryExpression.Operand;
            }
        }
    }
}