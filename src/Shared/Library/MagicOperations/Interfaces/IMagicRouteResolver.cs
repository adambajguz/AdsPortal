namespace MagicOperations.Interfaces
{
    using MagicOperations.Schemas;

    public interface IMagicRouteResolver
    {
        OperationSchema? ResolveSchema(string route);
    }
}