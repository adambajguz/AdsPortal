namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public interface IOperationRenderer
    {
        object? ErrorModel { get; }
        object OperationModel { get; }
        OperationSchema OperationSchema { get; }
        object? ResponseModel { get; }

        string GetRouteToRealtedOperation(Type operationType, IReadOnlyDictionary<string, string>? arguments = null);
    }
}