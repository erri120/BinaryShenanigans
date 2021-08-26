using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.Steps;

namespace BinaryShenanigans.BinaryParser
{
    internal class Parser<T> : IBinaryParser<T>, IDisposable
    {
        private readonly List<ABinaryParserStep<T>> _steps;

        private readonly T _currentObject;
        private byte[]? _buffer;
        //private BetterBinaryReader? _bbr;

        public Parser(List<ABinaryParserStep<T>> steps)
        {
            _steps = steps;

            var ctor = typeof(T)
                .GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .First();

            var newExpression = Expression.New(ctor);
            
            _currentObject = Activator.CreateInstance<T>();
        }

        public Parser(Expression<Func<byte[], T>> result)
        {
            throw new NotImplementedException();
        }

        //public BetterBinaryReader GetBinaryReader() => _bbr ?? throw new NotImplementedException();
        
        public T Parse(byte[] data)
        {
            _buffer = data;
            //_bbr = new BetterBinaryReader(_buffer);

            ExecuteSteps(_steps);
            return _currentObject;
        }

        public T GetCurrentObject() => _currentObject;

        public void SkipBytes(ulong count)
        {
            //GetBinaryReader().SkipBytes((int)count);
        }

        public void ExecuteSteps(IEnumerable<ABinaryParserStep<T>> steps)
        {
            foreach (var step in steps)
            {
                if (step.ShouldExecute(this))
                    step.Execute(this);
            }
        }

        public void Dispose()
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            //_bbr?.Dispose();
        }
    }
}