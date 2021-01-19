namespace AdsPortal.WebPortal.Services.Auth
{
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Models;

    public interface IAccountService
    {
        Task<bool> LoginAsync(TokenModel authData);
        Task<bool> LogoutAsync();
    }
}
