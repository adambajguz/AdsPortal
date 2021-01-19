namespace AdsPortal.WebPortal.Services.Auth
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Models;
    using Blazored.SessionStorage;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        private readonly ISessionStorageService _sessionStorage;
        private readonly HttpClient _httpClient;

        public AccountService(ISessionStorageService sessionStorage,
                              AuthenticationStateProvider customAuthenticationProvider,
                              HttpClient httpClient)
        {
            _sessionStorage = sessionStorage;
            _customAuthenticationProvider = customAuthenticationProvider;
            _httpClient = httpClient;
        }
        public async Task<bool> LoginAsync(TokenModel authData)
        {
            await _sessionStorage.SetItemAsync("token", authData.Token);
            await _sessionStorage.SetItemAsync("refreshToken", authData.RefreshToken);
            (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            await _sessionStorage.RemoveItemAsync("token");
            (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();
            return true;
        }
    }
}
