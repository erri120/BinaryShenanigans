using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.Steps;
using JetBrains.Annotations;

namespace BinaryShenanigans.BinaryParser
{
    [PublicAPI]
    public static class BinaryParserBuilder
    {
        public static IBinaryParserBuilder<T> Configure<T>() => new BinaryParserBuilder<T>();
    }
    
    [PublicAPI]
    public partial class BinaryParserBuilder<T> : IBinaryParserBuilder<T>
    {
        internal BinaryParserBuilder() { }

        private readonly List<ABinaryParserStep<T>> _steps = new();

        public IBinaryParser<T> CreateParser()
        {
            var parameter = Expression.Parameter(typeof(byte[]), "data");
            
            var instanceVariable = Expression.Variable(typeof(T), "instance");

            var constructorInfo = typeof(T)
                .GetConstructors()
                .OrderBy(x => x.GetParameters().Length)
                .First();

            var getNewInstance = Expression.New(constructorInfo);
            var assign = Expression.Assign(instanceVariable, getNewInstance);

            var mainBlock = CreateBlock(parameter);
            
            var body = Expression.Block(typeof(T),
                variables: mainBlock.Variables.Prepend(instanceVariable),
                expressions: mainBlock.Expressions.Prepend(assign));
            
            var result = Expression.Lambda<Func<byte[], T>>(body, parameter);
            return new Parser<T>(result);
        }

        private BlockExpression CreateBlock(Expression dataExpression)
        {
            throw new NotImplementedException();
            /*var expressions = new List<Expression>();
            
            Expression<Func<byte[], BetterBinaryReader>> newBetterBinaryReader = bytes => new BetterBinaryReader(bytes, Encoding.UTF8, true);
            var createBetterBinaryReaderFunc = Expression.Variable(typeof(Func<byte[], BetterBinaryReader>), "createBetterBinaryReader");
            var assignBetterBinaryReaderFunc = Expression.Assign(createBetterBinaryReaderFunc, newBetterBinaryReader);

            var betterBinaryReader = Expression.Variable(typeof(BetterBinaryReader), "bbr");
            expressions.Add(Expression.Assign(betterBinaryReader, newBetterBinaryReader));
            
            
            
            var result = Expression.Block(expressions);
            return result;*/
        }
        
        
        
        public IBinaryParserBuilderIfBranch<T> If(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
        
        public IBinaryParserBuilderWithConditionBranch<T> WithCondition(Expression<Func<T, bool>> conditionExpression,
            Expression<Func<IBinaryParserBuilder<T>, IBinaryParserBuilder<T>>> conditionMetExpression)
        {
            throw new NotImplementedException();
        }
        
        public IBinaryParserBuilder<T> SkipBytes(ulong count)
        {
            _steps.Add(new SkipBytesStep<T>(count));
            return this;
        }
    }
}