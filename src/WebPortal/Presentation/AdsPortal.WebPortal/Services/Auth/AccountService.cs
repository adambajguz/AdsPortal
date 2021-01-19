namespace AdsPortal.WebPortal.Services.Auth
{
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Models;
    using Blazored.SessionStorage;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        private readonly ISessionStorageService _sessionStorage;

        public AccountService(ISessionStorageService sessionStorage,
                              AuthenticationStateProvider customAuthenticationProvider)
        {
            _sessionStorage = sessionStorage;
            _customAuthenticationProvider = customAuthenticationProvider;
        }
        public async Task<bool> LoginAsync(TokenModel authData)
        {
            await _sessionStorage.SetItemAsync("token", authData.Token);
            await _sessionStorage.SetItemAsync("refresh_token", authData.RefreshToken);
            (_customAuthenticationProvider as CustomAuthenticationProvider)?.Notify(); //TODO fix

            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            await _sessionStorage.RemoveItemAsync("token");
            await _sessionStorage.RemoveItemAsync("refresh_token");

            (_customAuthenticationProvider as CustomAuthenticationProvider)?.Notify();

            return true;
        }
    }
}
