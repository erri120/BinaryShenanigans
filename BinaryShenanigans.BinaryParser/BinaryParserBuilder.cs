using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.ReadSteps;
using CodeWriterUtils;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser
{
    [PublicAPI]
    public static class BinaryParserBuilder
    {
        public static IBinaryParserBuilder<T> Configure<T>() => new BinaryParserBuilder<T>();
    }

    internal abstract class ABinaryParserBuilder
    {
        public abstract Type Type { get; }
        public abstract List<AReadStep> Steps { get; }

        public void WriteCode(CodeWriter codeWriter)
        {
            foreach (var step in Steps)
            {
                step.WriteCode(codeWriter);
            }
        }
    }

    [PublicAPI]
    internal partial class BinaryParserBuilder<T> : ABinaryParserBuilder, IBinaryParserBuilder<T>
    {
        public override Type Type => typeof(T);

        private readonly List<AReadStep> _steps = new();
        public override List<AReadStep> Steps => _steps;

        private readonly BinaryParserBuilder<T>? _parent;

        public BinaryParserBuilder() { }

        public BinaryParserBuilder(BinaryParserBuilder<T> parent)
        {
            _parent = parent;
        }

        public IBinaryParserBuilder<T> CustomLogic(CustomLogicDelegate<T> customLogicDelegate)
        {
            var methodInfo = customLogicDelegate.Method;
            if (!methodInfo.Attributes.HasFlag(MethodAttributes.Public))
                throw new NotImplementedException();
            if (!methodInfo.Attributes.HasFlag(MethodAttributes.Static))
                throw new NotImplementedException();

            _steps.Add(new CustomLogicStep(methodInfo));
            return this;
        }

        public IBinaryParserBuilder<T> SkipBytes(ulong count)
        {
            _steps.Add(new SkipBytesStep(count));
            return this;
        }

        public IBinaryParserBuilder<T> ReadOther<TOther>(Expression<Func<T, TOther>> expression, Type otherConfigurationType)
        {
            _steps.Add(new ReadOtherStep(expression, otherConfigurationType));
            return this;
        }
    }
}
