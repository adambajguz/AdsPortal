namespace AdsPortal.WebPortal.Services
{
    using System;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Configurations;
    using AdsPortal.WebPortal.Models.MediaItem;
    using Markdig;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class MediaService : IMediaService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public MediaService(HttpClient httpClient, ILogger<MediaService> logger, IOptions<ApplicationConfiguration> applicationConfiguration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration.Value;
        }

        public async Task<MediaItemDetails?> GetMediaDetails(Guid? id, CancellationToken cancellationToken = default)
        {
            return id is Guid g ? await GetMediaDetails(g, cancellationToken) : null;
        }

        public async Task<MediaItemDetails?> GetMediaDetails(Guid id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response;
            try
            {
                string path = $"{ _applicationConfiguration.ApiUrl}media/get-by-id/{id}";

                response = await _httpClient.GetAsync(path, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                    return JsonConvert.DeserializeObject<MediaItemDetails>(responseString);
                }
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error.");
                return null;
            }

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    string error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("Failed to get media item: {StatusCode} {Error}.", response.StatusCode, error);
                    return null;

                case System.Net.HttpStatusCode.Unauthorized:
                case System.Net.HttpStatusCode.Forbidden:
                case System.Net.HttpStatusCode.NotFound:
                    _logger.LogError("Failed to get media item: {StatusCode}.", response.StatusCode);
                    return null;

                default:
                    return null;
            }
        }
    }
}
