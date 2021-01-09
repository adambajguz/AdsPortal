namespace MagicOperations.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using MagicOperations.Interfaces;
    using MagicOperations.Internal;
    using MagicOperations.Schemas;

    public sealed class OperationModelFactory : IOperationModelFactory
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> _cachedFunctions = new ConcurrentDictionary<Type, Func<object>>();

        public OperationModelFactory()
        {

        }

        public object CreateInstanceAndBindData(Type modelType, IEnumerable<OperationUriArgument> arguments)
        {
            object model = CreateInstance(modelType);

            foreach (OperationUriArgument arg in arguments)
            {
                PropertyInfo propertyInfo = arg.Schema.Property;

                object? value = arg.Convert();

                propertyInfo.SetValue(model, value);
            }

            return model;
        }

        public object CreateInstance(Type type)
        {
            if (_cachedFunctions.TryGetValue(type, out Func<object>? cachedFunction))
            {
                return cachedFunction();
            }

            ConstructorInfo constructor = type.GetConstructor(Array.Empty<Type>()) ?? throw new NullReferenceException($"Parameterless constructor not found in type {type.AssemblyQualifiedName}.");
            NewExpression newExpression = Expression.New(constructor);
            Expression<Func<object>> lambdaExprssion = Expression.Lambda<Func<object>>(newExpression);

            Func<object> func = lambdaExprssion.Compile();
            _cachedFunctions.TryAdd(type, func);

            return func();
        }
    }
}
