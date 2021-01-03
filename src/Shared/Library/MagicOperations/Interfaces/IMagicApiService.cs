namespace MagicOperations.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMagicApiService
    {
        Task<object?> ExecuteAsync(object model, CancellationToken cancellationToken = default);
    }
}