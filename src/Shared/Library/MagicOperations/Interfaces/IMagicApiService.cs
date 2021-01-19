namespace MagicOperations.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMagicApiService
    {
        Task<TResponse?> ExecuteAsync<TOperation, TResponse>(TOperation model, bool forceGet = false, CancellationToken cancellationToken = default)
            where TOperation : notnull;
    }
}