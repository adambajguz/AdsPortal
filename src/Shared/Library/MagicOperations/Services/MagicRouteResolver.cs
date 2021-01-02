namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MagicOperations.Schemas;

    public class MagicRouteResolver : IMagicRouteResolver
    {
        private readonly MagicOperationsConfiguration _configuration;

        public MagicRouteResolver(MagicOperationsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (OperationSchema Schema, IEnumerable<OperationArgument>)? Resolve(string route)
        {
            string[] parts = route.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Empty route.", nameof(route));

            if (parts.Length >= 2)
            {
                OperationGroupSchema? group = _configuration.OperationGroups.Values.Where(x => x.Route == parts[0]).FirstOrDefault();

                if (group is not null && group.Operations.FirstOrDefault(x => x.MatchesRoute(route)) is OperationSchema os0)
                {
                    return (os0, os0.ExtractArguments(route)!);
                }
            }

            if (_configuration.OperationSchemas.FirstOrDefault(x => x.MatchesRoute(route)) is OperationSchema os1)
            {
                return (os1, os1.ExtractArguments(route)!);
            }

            return null;
        }
    }
}
