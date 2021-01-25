using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdsPortal.WebApi.Client
{
    public class BaseWebApiClient : IBaseWebApiClient
    {
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HttpRequestMessage());
        }

        protected Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url)
        {
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
