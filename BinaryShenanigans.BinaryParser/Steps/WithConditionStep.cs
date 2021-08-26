using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BinaryShenanigans.BinaryParser.Steps
{
    internal class WithConditionStep<T> : ABinaryParserStep<T>
    {
        private readonly Func<T, bool> _conditionFunc;
        private readonly List<ABinaryParserStep<T>> _steps;

        public WithConditionStep(Expression<Func<T, bool>> conditionExpression, List<ABinaryParserStep<T>> steps)
        {
            _conditionFunc = conditionExpression.Compile();
            _steps = steps;
        }
        
        public override bool ShouldExecute(Parser<T> parser)
        {
            var current = parser.GetCurrentObject();
            return _conditionFunc(current);
        }

        public override void Execute(Parser<T> parser)
        {
            parser.ExecuteSteps(_steps);
        }
    }
}