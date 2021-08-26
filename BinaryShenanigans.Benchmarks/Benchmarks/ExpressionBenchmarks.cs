using System;
using System.Linq;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;

namespace BinaryShenanigans.Benchmarks.Benchmarks
{
    public class SomeClass
    {
        public uint UInt32Field;
        public ulong UInt64Field;
        
        public SomeClass() {}
    }
    
    [MemoryDiagnoser]
    public class ExpressionBenchmarks
    {
        private readonly Func<SomeClass> _compiledFunc;

        public ExpressionBenchmarks()
        {
            _compiledFunc = GetFunc();
        }

        [Benchmark(Baseline = true)]
        public SomeClass CreateClassManually()
        {
            return new SomeClass();
        }

        [Benchmark]
        public SomeClass CreateClassWithActivator()
        {
            return Activator.CreateInstance<SomeClass>();
        }
        
        [Benchmark]
        public SomeClass CreateClassWithExpression()
        {
            return GetFunc()();
        }

        [Benchmark]
        public SomeClass CreateClassWithCompiledExpression()
        {
            return _compiledFunc();
        }

        private static Func<SomeClass> GetFunc()
        {
            var constructorInfo = typeof(SomeClass)
                .GetConstructors()
                .First();

            var instanceVariable = Expression.Variable(typeof(SomeClass), "inst");
            var newExpression = Expression.New(constructorInfo);
            var assign = Expression.Assign(instanceVariable, newExpression);

            var body = Expression.Block(
                typeof(SomeClass),
                variables: new []{instanceVariable},
                expressions: assign);

            return Expression.Lambda<Func<SomeClass>>(body).Compile();
        }
    }
}