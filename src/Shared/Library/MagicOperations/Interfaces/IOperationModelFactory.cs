namespace MagicOperations.Interfaces
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public interface IOperationModelFactory
    {
        object CreateInstanceAndBindData(Type modelType, IEnumerable<OperationUriArgument> arguments);
        object CreateInstance(Type type);
    }
}