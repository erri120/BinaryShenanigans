using System;
using System.Linq.Expressions;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.ReadSteps;

namespace BinaryShenanigans.BinaryParser
{
    internal partial class BinaryParserBuilder<T>
    {
        public IBinaryParserBuilderIfBranch<T> If(Expression<Func<T, bool>> expression)
        {
            var step = new IfBranchStep(expression);
            _steps.Add(step);

            var ifBranch = new IfBranch<T>(step, this);
            return ifBranch;
        }
    }

    internal class IfBranch<T> : IBinaryParserBuilderIfBranch<T>, IBinaryParserBuilderIfBranchWhenTruePath<T>, IBinaryParserBuilderIfBranchWhenFalsePath<T>
    {
        private readonly IBinaryParserBuilder<T> _parentBuilder;
        private readonly IfBranchStep _ifBranchStep;

        public IfBranch(IfBranchStep ifBranchStep, IBinaryParserBuilder<T> parentBuilder)
        {
            _parentBuilder = parentBuilder;
            _ifBranchStep = ifBranchStep;
        }

        public IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T> WhenTrue()
        {
            _ifBranchStep.WhenTrueBuilder = null;
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T> WhenFalse()
        {
            _ifBranchStep.WhenFalseBuilder = null;
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenTruePath<T> WhenTrue(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func)
        {
            SetWhenTrue(func);
            return this;
        }

        public IBinaryParserBuilderIfBranchWhenFalsePath<T> WhenFalse(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func)
        {
            SetWhenFalse(func);
            return this;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenTrueEmptyPath<T>.WhenFalse(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func)
        {
            SetWhenFalse(func);
            return _parentBuilder;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenTruePath<T>.WhenFalse()
        {
            _ifBranchStep.WhenFalseBuilder = null;
            return _parentBuilder;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenFalseEmptyPath<T>.WhenTrue(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func)
        {
            SetWhenTrue(func);
            return _parentBuilder;
        }

        IBinaryParserBuilder<T> IBinaryParserBuilderIfBranchWhenFalsePath<T>.WhenTrue()
        {
            _ifBranchStep.WhenTrueBuilder = null;
            return _parentBuilder;
        }

        private void SetWhenFalse(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func)
        {
            var whenFalseBuilder = new BinaryParserBuilder<T>();
            _ifBranchStep.WhenFalseBuilder = (ABinaryParserBuilder)func(whenFalseBuilder);
        }

        private void SetWhenTrue(Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>> func)
        {
            var whenTrueBuilder = new BinaryParserBuilder<T>();
            _ifBranchStep.WhenTrueBuilder = (ABinaryParserBuilder)func(whenTrueBuilder);
        }
    }
}
