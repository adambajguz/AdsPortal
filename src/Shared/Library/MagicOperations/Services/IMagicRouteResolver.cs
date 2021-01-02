namespace MagicOperations.Services
{
    using System.Collections.Generic;
    using MagicOperations.Schemas;

    public interface IMagicRouteResolver
    {
        (OperationSchema Schema, IEnumerable<OperationArgument>)? Resolve(string route);
    }
}