using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinaryShenanigans.BinaryParser.Gen
{
    public static class ReflectionUtils
    {
        public static List<TypeInfo> FindInterfaceImplementations(Assembly assembly, Type interfaceType)
        {
            return assembly.DefinedTypes
                .Where(x => x.ImplementedInterfaces.Any(i => i.GUID.Equals(interfaceType.GUID)))
                .ToList();
        }

        public static T InvokeMethod<T>(TypeInfo classTypeInfo, string methodName)
        {
            var classInstance = Activator.CreateInstance(classTypeInfo);
            if (classInstance == null)
                throw new NotImplementedException();

            var methodInfo = classTypeInfo.GetMethod(methodName);
            if (methodInfo == null)
                throw new NotImplementedException();

            var result = methodInfo.Invoke(classInstance, null);
            if (result == null)
                throw new NotImplementedException();

            return (T)result;
        }
    }
}
