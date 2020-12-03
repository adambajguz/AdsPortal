namespace AdsPortal.CLI.Commands.User
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Domain.Jwt;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user create")]
    public class CreateUserCommand : ICommand
    {
        [CommandOption("email", 'e')]
        public string? Email { get; init; }

        [CommandOption("password", 'p')]
        public string? Password { get; init; }

        [CommandOption("name", 'n')]
        public string? Name { get; init; }

        [CommandOption("surname", 's')]
        public string? Surname { get; init; }

        [CommandOption("phone-number")]
        public string? PhoneNumber { get; init; }

        [CommandOption("address", 'a')]
        public string? Address { get; init; }

        [CommandOption("role", 'r')]
        public Roles Role { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public CreateUserCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("user/create", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
