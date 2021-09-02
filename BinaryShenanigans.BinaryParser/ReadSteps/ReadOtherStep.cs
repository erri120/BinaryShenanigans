using System;
using System.Linq.Expressions;
using System.Reflection;
using CodeWriterUtils;

namespace BinaryShenanigans.BinaryParser.ReadSteps
{
    internal class ReadOtherStep : AReadStep
    {
        private readonly Expression _expression;
        private readonly Type _otherConfigurationType;

        public ReadOtherStep(Expression expression, Type otherConfigurationType)
        {
            _expression = expression;
            _otherConfigurationType = otherConfigurationType;
        }

        public override void WriteCode(CodeWriter codeWriter)
        {
            var memberInfo = ExpressionUtils.GetMemberInfoFromExpression(_expression);
            if (memberInfo == null)
                throw new NotImplementedException();

            var memberName = memberInfo.Name;

            Type memberType;
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                var propertyInfo = (PropertyInfo)memberInfo;
                memberType = propertyInfo.PropertyType;
            } else if (memberInfo.MemberType == MemberTypes.Field)
            {
                var fieldInfo = (FieldInfo)memberInfo;
                memberType = fieldInfo.FieldType;
            }
            else
            {
                throw new NotImplementedException();
            }

            codeWriter.WriteLine($"res.{memberName} = {memberType.Name}Parser.ParseStatic(span.Slice(reader.Position));");
        }
    }
}
