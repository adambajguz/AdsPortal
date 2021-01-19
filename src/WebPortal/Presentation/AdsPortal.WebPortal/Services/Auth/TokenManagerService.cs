namespace AdsPortal.WebPortal.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AdsPortal.WebPortal.Models;
    using Blazored.SessionStorage;

    public class TokenManagerService : ITokenManagerService
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorage;

        public TokenManagerService(HttpClient httpClient, ISessionStorageService sessionStorage)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
        }

        private bool ValidateTokenExpiration(string token)
        {
            List<Claim> claims = JwtParser.ParseClaimsFromJwt(token).ToList();
            if (claims?.Count == 0)
            {
                return false;
            }
            string expirationSeconds = claims.Where(_ => _.Type.ToLower() == "exp").Select(_ => _.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(expirationSeconds))
            {
                return false;
            }

            var exprationDate = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expirationSeconds));
            if (exprationDate < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        private async Task<string> RefreshTokenEndPoint(TokenModel tokenModel)
        {
            var response = await _httpClient.PostAsJsonAsync<TokenModel>("/account/activate-token-by-refreshtoken", tokenModel);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }
            TokenModel authResponse = await response.Content.ReadFromJsonAsync<TokenModel>();
            await _sessionStorage.SetItemAsync<string>("token", authResponse.Token);
            await _sessionStorage.SetItemAsync<string>("refreshToken", authResponse.RefreshToken);
            return authResponse.Token;
        }

        public async Task<string> GetTokenAsync()
        {
            string token = await _sessionStorage.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token))
            {
                return string.Empty;
            }

            if (ValidateTokenExpiration(token))
            {
                return token;
            }

            string refreshToken = await _sessionStorage.GetItemAsync<string>("refreshToken");
            if (string.IsNullOrEmpty(refreshToken))
            {
                return string.Empty;
            }

            TokenModel tokenModel = new TokenModel { Token = token, RefreshToken = refreshToken };
            return await RefreshTokenEndPoint(tokenModel);
        }
    }
}
