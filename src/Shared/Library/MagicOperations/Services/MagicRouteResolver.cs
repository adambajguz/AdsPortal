namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MagicOperations.Schemas;

    internal class MagicRouteResolver : IMagicRouteResolver
    {
        private readonly MagicOperationsConfiguration _configuration;

        public MagicRouteResolver(MagicOperationsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (OperationSchema Schema, IEnumerable<OperationArgument>)? Resolve(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                throw new ArgumentException("Empty route.", nameof(route));

            //Fast group based match
            OperationGroupSchema? group = _configuration.OperationGroups.Values.Where(x => x.Route is not null && route.StartsWith(x.Route))
                                                                               .FirstOrDefault();

            if (group is OperationGroupSchema g && g.Operations.FirstOrDefault(x => x.MatchesRoute(route)) is OperationSchema os0)
            {
                return (os0, os0.ExtractArguments(route)!);
            }

            //Slow all routes match when group has no operation group route specified
            if (_configuration.OperationSchemas.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Group.Route) && x.MatchesRoute(route)) is OperationSchema os1)
            {
                return (os1, os1.ExtractArguments(route)!);
            }

            return null;
        }
    }
}
