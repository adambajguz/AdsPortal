namespace AdsPortal.CLI.Commands.User
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user auth")]
    public class AuthenticateCommand : ICommand
    {
        [CommandParameter(0)]
        public string? Email { get; init; }

        [CommandParameter(1)]
        public string? Password { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;
        private readonly AuthTokenHolder _authTokenHolder;

        public AuthenticateCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider, AuthTokenHolder authTokenHolder)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
            _authTokenHolder = authTokenHolder;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("user/auth", this, console.GetCancellationToken());

            JwtTokenModel? model = await response.Content.ReadAsAsync<JwtTokenModel>();
            _authTokenHolder.Set(model);

            await response.PrintResponse(console);
        }
    }
}
