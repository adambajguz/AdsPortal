namespace AdsPortal.WebPortal.Services.Auth
{
    using System.Threading.Tasks;

    public interface ITokenManagerService
    {
        Task<string> GetTokenAsync();
    }
}
