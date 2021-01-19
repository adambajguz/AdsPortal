namespace AdsPortal.WebPortal.Services.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Blazored.SessionStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Server;

    public class CustomAuthenticationProvider : ServerAuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorage;

        public CustomAuthenticationProvider(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //https://stackoverflow.com/questions/60383601/how-to-authenticate-a-user-with-blazor-server
            //https://stackoverflow.com/questions/58387683/signin-for-blazor-server-side-app-not-working/58390813#58390813
            string token = await _sessionStorage.GetItemAsync<string>("token");

            if (string.IsNullOrEmpty(token))
            {
                var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity() { }));
                return anonymous;
            }

            var userClaimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "Fake Authentication"));
            var loginUser = new AuthenticationState(userClaimPrincipal);

            return loginUser;
        }

        public void Notify()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
