namespace AdsPortal.WebPortal.Services.Auth
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Models;
    using Blazored.SessionStorage;
    using Microsoft.AspNetCore.Components.Authorization;

    public class AccountService : IAccountService
    {
        private readonly AuthenticationStateProvider _customAuthenticationProvider;
        private readonly ISessionStorageService _sessionStorage;
        private readonly JwtSecurityTokenHandler _handler;

        public AccountService(ISessionStorageService sessionStorage,
                              AuthenticationStateProvider customAuthenticationProvider)
        {
            _sessionStorage = sessionStorage;
            _customAuthenticationProvider = customAuthenticationProvider;

            _handler = new JwtSecurityTokenHandler();
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

        public async Task<Guid> GetUserIdFromToken()
        {
            string? token = await _sessionStorage.GetItemAsync<string>("token");

            if (token is null)
                return Guid.Empty;

            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim? claim = secToken.Claims?.FirstOrDefault(x => x.Type.Equals("nameidentifier") || x.Type.Equals("nameid") || x.Type.Equals(ClaimTypes.NameIdentifier));
            Guid userId = Guid.Parse(claim?.Value!);

            return userId;
        }

        public async Task<string> GetUserEmailFromToken()
        {
            string? token = await _sessionStorage.GetItemAsync<string>("token");

            if (token is null)
                return string.Empty;

            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim? claim = secToken.Claims?.FirstOrDefault(x => x.Type.Equals("emailaddress") || x.Type.Equals("email") || x.Type.Equals(ClaimTypes.Email));

            return claim?.Value ?? string.Empty;
        }

        public async Task<string> GetUserNameFromToken()
        {
            string? token = await _sessionStorage.GetItemAsync<string>("token");

            if (token is null)
                return string.Empty;

            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim? claim = secToken.Claims?.FirstOrDefault(x => x.Type.Equals("name") || x.Type.Equals("unique_name") || x.Type.Equals(ClaimTypes.Name));

            return claim?.Value ?? string.Empty;
        }

        public async Task<string> GetUserSurnameFromToken()
        {
            string? token = await _sessionStorage.GetItemAsync<string>("token");

            if (token is null)
                return string.Empty;

            JwtSecurityToken secToken = _handler.ReadJwtToken(token);
            Claim? claim = secToken.Claims?.FirstOrDefault(x => x.Type.Equals("surname") || x.Type.Equals("family_name") || x.Type.Equals(ClaimTypes.Surname));

            return claim?.Value ?? string.Empty;
        }
    }
}
