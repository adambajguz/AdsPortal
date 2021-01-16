namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;

    public interface IOperationRenderer
    {
        OperationContext Context { get; }

        object? ErrorModel { get; }
        object OperationModel { get; }
        object? ResponseModel { get; }

        string GetRouteToRealtedOperation(Type operationType, IReadOnlyDictionary<string, string>? arguments = null);
    }
}