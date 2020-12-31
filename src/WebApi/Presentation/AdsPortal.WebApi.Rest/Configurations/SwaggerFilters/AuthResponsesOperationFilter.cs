namespace AdsPortal.WebApi.Rest.Configurations.SwaggerFilters
{
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ImmutableList<AuthorizeAttribute>? authAttributes = context?.MethodInfo
                                                                       ?.DeclaringType
                                                                       ?.GetCustomAttributes(true)
                                                                        .Union(context.MethodInfo.GetCustomAttributes(true))
                                                                        .OfType<AuthorizeAttribute>()
                                                                        .ToImmutableList();

            if (authAttributes is null)
                return;

            if (authAttributes.Count > 0)
            {
                string str = BuildString(authAttributes);
                operation.Summary += $" {str}";
                operation.Description += $"<br><br>Only users with role {str} can access this endpoint.";
            }
        }

        private static string BuildString(ImmutableList<AuthorizeAttribute> authAttributes)
        {
            string[] roles = authAttributes.Select(x => string.Concat("[",
                                                                      x.Roles?.Replace(", ", "|") ?? string.Empty,
                                                                      "]"))
                                           .ToArray();

            return string.Join(" & ", roles);
        }
    }
}
