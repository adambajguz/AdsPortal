namespace AdsPortal.CLI.Commands.User
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Domain.Jwt;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user update")]
    public class UpdateUserCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        [CommandOption("email", 'e')]
        public string? Email { get; init; }

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

        public UpdateUserCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PutAsJsonAsync("user/update", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
