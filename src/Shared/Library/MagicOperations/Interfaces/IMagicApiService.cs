namespace MagicOperations.Interfaces
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMagicApiService
    {
        Task<TResponse?> ExecuteAsync<TOperation, TResponse>(TOperation model, bool forceGet = false, CancellationToken cancellationToken = default)
            where TOperation : notnull;

        Task<TResponse?> SendAsync<TRequest, TResponse>(TRequest requestData, HttpMethod httpMethod, string path, CancellationToken cancellationToken = default)
            where TRequest : notnull
            where TResponse : class;

        Task<TResponse?> SendAsync<TResponse>(HttpMethod httpMethod, string path, CancellationToken cancellationToken = default)
            where TResponse : class;
    }
}