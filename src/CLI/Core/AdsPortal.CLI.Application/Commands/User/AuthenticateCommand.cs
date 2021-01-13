namespace AdsPortal.CLI.Application.Commands.User
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using AdsPortal.CLI.Application.Models;
    using AdsPortal.CLI.Application.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user auth", Description = "Login a user.")]
    public class AuthenticateCommand : ICommand
    {
        [CommandParameter(0)]
        public string? Email { get; init; }

        [CommandParameter(1)]
        public string? Password { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthTokenHolder _authTokenHolder;

        public AuthenticateCommand(IHttpClientFactory httpClientFactory, AuthTokenHolder authTokenHolder)
        {
            _httpClientFactory = httpClientFactory;
            _authTokenHolder = authTokenHolder;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("user/auth", this, console.GetCancellationToken());

            AuthenticateUserResponse? model = await response.Content.ReadFromJsonAsync<AuthenticateUserResponse>();
            _authTokenHolder.Set(model);

            await response.PrintResponse(console);
        }
    }
}
