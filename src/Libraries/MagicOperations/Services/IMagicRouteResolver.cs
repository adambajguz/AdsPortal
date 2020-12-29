namespace MagicOperations.Services
{
    using MagicOperations.Schemas;

    public interface IMagicRouteResolver
    {
        (OperationSchema Schema, UriTemplate.UriTemplateMatch Arguments)? Resolve(string route);
    }
}