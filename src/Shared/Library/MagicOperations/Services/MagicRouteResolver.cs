namespace MagicOperations.Services
{
    using System.Linq;
    using MagicOperations.Interfaces;
    using MagicOperations.Schemas;

    internal sealed class MagicRouteResolver : IMagicRouteResolver
    {
        private readonly MagicOperationsConfiguration _configuration;

        public MagicRouteResolver(MagicOperationsConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OperationSchema? ResolveSchema(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return null;

            //Fast group based match
            OperationGroupSchema? group = _configuration.OperationGroups.Values.Where(x => x.Path is not null && route.StartsWith(x.Path))
                                                                               .FirstOrDefault();

            if (group is OperationGroupSchema g && g.Operations.FirstOrDefault(x => x.MatchesPath(route)) is OperationSchema os0)
            {
                return os0;
            }

            //Slow all routes match when group has no operation group route specified
            if (_configuration.OperationSchemas.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Group.Path) && x.MatchesPath(route)) is OperationSchema os1)
            {
                return os1;
            }

            return null;
        }
    }
}
