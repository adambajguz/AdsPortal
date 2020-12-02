namespace AdsPortal.ManagementUI.Services
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Blazored.LocalStorage;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using MediatR;

    //https://chrissainty.com/building-a-blogging-app-with-blazor-adding-authentication/
    public class AppStateService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IMediator _mediator;

        public bool IsLoggedIn { get; private set; }

        public AppStateService(HttpClient httpClient, ILocalStorageService localStorage, IMediator mediator)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _mediator = mediator;
        }

        //public async Task Login(LoginRequest model)
        //{
        //    try
        //    {
        //        JwtTokenModel? response = await _mediator.Send(new GetAuthenticationTokenQuery(model));

        //        await SaveToken(response);
        //        await SetAuthorizationHeader();
        //        IsLoggedIn = true;
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            IsLoggedIn = false;
        }

        private async Task SaveToken(JwtTokenModel? response)
        {
            await _localStorage.SetItemAsync("authToken", response.Token);
        }

        private async Task SetAuthorizationHeader()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                string? token = await _localStorage.GetItemAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
