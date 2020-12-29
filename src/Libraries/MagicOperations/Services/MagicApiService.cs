namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;
    using MagicOperations.Schemas;

    public class MagicApiService
    {
        private static readonly Regex _regex = new Regex(@"(?<=\{)[^}{]*(?=\})", RegexOptions.IgnoreCase);

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MagicOperationsConfiguration _configuration;

        public MagicApiService(IHttpClientFactory httpClientFactory, MagicOperationsConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(object model, CancellationToken cancellationToken = default)
        {
            Type type = model.GetType();
            OperationSchema? schema = _configuration.ModelToSchemaMappings.GetValueOrDefault(type);

            _ = schema ?? throw new MagicOperationsException($"Invalid schema.");

            string route = ReplaceTokens(model, schema);
            string path = Path.Combine(schema.Group.Key ?? string.Empty, route);

            HttpClient client = _httpClientFactory.CreateClient("MagicOperationsAPI");
            var response = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(path),
                Method = new HttpMethod(schema.HttpMethod)
            }, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var obj = response.Content.ReadAsStringAsync();
            }
        }

        public string ReplaceTokens(object model, OperationSchema schema)
        {
            string route = schema.Action ?? string.Empty;
            MatchCollection matches = _regex.Matches(route);

            List<string> tokens = matches.Cast<Match>()
                                         .Select(m => m.Value)
                                         .Distinct()
                                         .ToList();

            string patchedRoute = route;

            foreach (string t in tokens)
            {
                PropertyInfo propertyInfo = schema.PropertySchemas.Where(x => string.Equals(t, x.Property.Name, StringComparison.Ordinal))
                                                                  .First().Property;

                string value = propertyInfo.GetValue(model)?.ToString() ?? string.Empty;

                patchedRoute = patchedRoute.Replace($"{{{t}}}", value);
            }

            return patchedRoute;
        }
    }
}
