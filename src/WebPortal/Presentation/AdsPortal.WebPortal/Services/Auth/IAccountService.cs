namespace AdsPortal.WebPortal.Services.Auth
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Models;

    public interface IAccountService
    {
        Task<bool> LoginAsync(TokenModel authData);
        Task<bool> LogoutAsync();

        Task<Guid> GetUserIdFromToken();
        Task<string> GetUserEmailFromToken();
        Task<string> GetUserNameFromToken();
        Task<string> GetUserSurnameFromToken();
    }
}
