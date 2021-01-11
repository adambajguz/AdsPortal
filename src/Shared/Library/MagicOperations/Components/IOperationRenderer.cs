namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public interface IOperationRenderer
    {
        string BasePath { get; }
        object? ErrorModel { get; }
        object OperationModel { get; }
        OperationSchema OperationSchema { get; }
        object? ResponseModel { get; }

        string GetRouteToOperation(Type operationType, IReadOnlyDictionary<string, string> arguments);
    }
}