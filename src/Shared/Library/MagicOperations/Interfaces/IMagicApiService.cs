namespace MagicOperations.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMagicApiService
    {
        Task<TResponse?> ExecuteAsync<TOperation, TResponse>(TOperation model, CancellationToken cancellationToken = default)
            where TOperation : notnull;
    }
}