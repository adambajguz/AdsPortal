namespace MagicOperations.Interfaces
{
    using System.Threading.Tasks;

    public interface ITokenManagerService
    {
        Task<string> GetTokenAsync();
    }
}
