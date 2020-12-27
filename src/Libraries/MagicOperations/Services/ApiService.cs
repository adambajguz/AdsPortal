namespace MagicOperations.Services
{
    using System.Net.Http;

    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //public async Task ExecuteAsync(OperationConfiguration operation, CancellationToken cancellationToken = default)
        //{
        //    HttpClient client = _httpClientFactory.CreateClient("MagicCRUDAPI");
        //    var response = await client.PostAsJsonAsync(operation.Template, this, cancellationToken);
        //}
    }
}
