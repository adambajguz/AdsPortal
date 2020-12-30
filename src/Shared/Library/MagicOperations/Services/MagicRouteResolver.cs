namespace MagicOperations.Services
{
    using System;
    using System.Linq;
    using MagicOperations.Schemas;

    public class MagicRouteResolver : IMagicRouteResolver
    {
        private readonly MagicOperationsConfiguration _configuration;

        public MagicRouteResolver(MagicOperationsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (OperationSchema Schema, UriTemplate.UriTemplateMatch Arguments)? Resolve(string route)
        {
            string[] parts = route.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Empty route.", nameof(route));

            Uri uriRoute = new Uri("http://localhost/" + route, UriKind.Absolute);

            if (parts.Length == 2)
            {
                OperationGroupSchema? group = _configuration.OperationGroups.Values.Where(x => x.Route == parts[1]).FirstOrDefault();

                if (group is not null && group.Operations.FirstOrDefault(x => x.MatchesRoute(uriRoute)) is OperationSchema os0)
                {
                    return (os0, os0.ExtractArguments(uriRoute));
                }
            }

            if (_configuration.OperationSchemas.FirstOrDefault(x => x.MatchesRoute(uriRoute)) is OperationSchema os1)
            {
                return (os1, os1.ExtractArguments(uriRoute));
            }

            return null;
        }
    }
}
