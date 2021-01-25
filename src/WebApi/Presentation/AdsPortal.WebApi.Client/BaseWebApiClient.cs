namespace AdsPortal.WebApi.Client
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "NSwag requirement")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "NSwag requirement")]
    public class BaseWebApiClient : IBaseWebApiClient
    {
        public string? JwtToken { get; set; }

        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HttpRequestMessage());
        }

        protected Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url)
        {
            if (!string.IsNullOrWhiteSpace(JwtToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JwtToken);
            }

            return Task.CompletedTask;
        }

        protected Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
        {
            return Task.CompletedTask;
        }

        protected Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response)
        {
            return Task.CompletedTask;
        }
    }
}
