namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
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

        public async Task CreateAsync(object model, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(model, cancellationToken);
        }

        public async Task UpdateAsync(object model, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(model, cancellationToken);
        }

        public async Task DeleteAsync(object model, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(model, cancellationToken);
        }

        public async Task<object?> Get(object model, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(model, cancellationToken);
        }

        public async Task<object?> GetList(object model, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(model, cancellationToken);
        }

        public async Task<object?> GetPaged(object model, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(model, cancellationToken);
        }

        public async Task<object?> ExecuteAsync(object model, CancellationToken cancellationToken = default)
        {
            Type type = model.GetType();
            OperationSchema? schema = _configuration.ModelToSchemaMappings.GetValueOrDefault(type);

            _ = schema ?? throw new MagicOperationsException($"Invalid schema.");

            string route = ReplaceTokens(model, schema);
            string path = Path.Combine(schema.Group.Key ?? string.Empty, route).Replace('\\', '/'); //TODO: add Url.Combine?

            HttpClient client = _httpClientFactory.CreateClient("MagicOperationsAPI");
            var response = await client.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(path, UriKind.Relative),
                Method = new HttpMethod(schema.HttpMethod),
                Content = JsonContent.Create(model)
            }, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                object? obj = schema.ResponseType is Type t ? await response.Content.ReadFromJsonAsync(t, cancellationToken: cancellationToken) : null;

                return obj;
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    string error = await response.Content.ReadAsStringAsync(cancellationToken);
                    throw new ApiException(error); //TODO: improve

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new ApiException("Unauthorized");

                case System.Net.HttpStatusCode.Forbidden:
                    throw new ApiException("Forbidden");

                case System.Net.HttpStatusCode.NotFound:
                    throw new ApiException("Not found");

                default:
                    throw new ApiException("Unknown error");
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
