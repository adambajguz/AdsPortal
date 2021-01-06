namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using MagicOperations.Schemas;
    using Microsoft.Extensions.Logging;

    public sealed class MagicApiService : IMagicApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISerializer _serializer;
        private readonly MagicOperationsConfiguration _configuration;
        private readonly ILogger _logger;

        public MagicApiService(IHttpClientFactory httpClientFactory, ISerializer serializer, MagicOperationsConfiguration configuration, ILogger<MagicApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _serializer = serializer;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TResponse?> ExecuteAsync<TOperation, TResponse>(TOperation operationModel, CancellationToken cancellationToken = default)
            where TOperation : notnull
        {
            OperationSchema? schema = _configuration.OperationTypeToSchemaMap.GetValueOrDefault(typeof(TOperation));

            _ = schema ?? throw new MagicOperationsException($"Invalid schema.");

            string route = schema.ResolvePath(operationModel);
            string path = Path.Combine(schema.Group.Key ?? string.Empty, route).Replace('\\', '/'); //TODO: add Url.Combine?

            HttpResponseMessage response;
            try
            {
                string serializedModel = _serializer.Serialize(operationModel);

                HttpClient client = _httpClientFactory.CreateClient("MagicOperationsAPI");
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(path, UriKind.Relative),
                    Method = new HttpMethod(schema.HttpMethod),
                    Content = new StringContent(serializedModel, Encoding.UTF8, MediaTypeNames.Application.Json),
                };

                response = await client.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                    return schema.ResponseType is null ? default : _serializer.Deserialize<TResponse>(responseString);
                }
            }
            catch (SerializerException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response.");
                throw new ApiException("Server response error", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error.");
                throw new ApiException("Server response error", ex);
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
    }
}
