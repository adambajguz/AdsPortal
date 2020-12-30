namespace AdsPortal.CLI.Application.Commands.User
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using AdsPortal.CLI.Application.Models;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user create", Description = "Create (register) a new user.")]
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

        public CreateUserCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("user/create", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
