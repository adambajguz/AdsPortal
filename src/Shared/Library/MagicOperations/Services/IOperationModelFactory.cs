namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public interface IOperationModelFactory
    {
        object CreateInstanceAndBindData(Type modelType, IEnumerable<OperationArgument> arguments);
        object CreateInstance(Type type);
    }
}