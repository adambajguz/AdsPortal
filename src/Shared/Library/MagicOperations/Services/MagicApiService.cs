namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mime;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class JsonIgnoreAttributeIgnorerContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            property.Ignored = false; // Here is the magic
            return property;
        }
    }

    public sealed class MagicApiService : IMagicApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISerializer _serializer;
        private readonly MagicOperationsConfiguration _configuration;
        private readonly ITokenManagerService _tokenManager;
        private readonly ILogger _logger;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public MagicApiService(IHttpClientFactory httpClientFactory,
                               ISerializer serializer,
                               MagicOperationsConfiguration configuration,
                               ITokenManagerService tokenManager,
                               AuthenticationStateProvider authenticationStateProvider,
                               ILogger<MagicApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _serializer = serializer;
            _configuration = configuration;
            _tokenManager = tokenManager;
            _authenticationStateProvider = authenticationStateProvider;
            _logger = logger;
        }

        public async Task<TResponse?> ExecuteAsync<TOperation, TResponse>(TOperation operationModel, bool forceGet = false, CancellationToken cancellationToken = default)
            where TOperation : notnull
        {
            OperationSchema? schema = _configuration.OperationTypeToSchemaMap.GetValueOrDefault(typeof(TOperation));

            _ = schema ?? throw new MagicOperationsException($"Invalid schema.");

            HttpResponseMessage response;
            try
            {
                string path = schema.GetFullPathFromModel(operationModel);

                if (forceGet)
                {
                    path = path.Replace("/update/", "/get/");
                }

                string serializedModel = _serializer.Serialize(operationModel);

                HttpClient client = _httpClientFactory.CreateClient("MagicOperationsAPI");
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri(path, UriKind.Relative),
                    Method = forceGet ? HttpMethod.Get : new HttpMethod(schema.HttpMethod),
                    Content = new StringContent(serializedModel, Encoding.UTF8, MediaTypeNames.Application.Json),
                };

                AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();

                if (authenticationState.User.Identity.IsAuthenticated)
                {
                    //request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, await _tokenManager.GetTokenAsync());
                }

                var token = await _tokenManager.GetTokenAsync();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
                }

                response = await client.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                    if (forceGet)
                    {
                        return JsonConvert.DeserializeObject<TResponse>(responseString, new JsonSerializerSettings { ContractResolver = new JsonIgnoreAttributeIgnorerContractResolver() });
                    }

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
