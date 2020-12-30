namespace AdsPortal.CLI.Application.Commands.User
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user change-password")]
    public class ChangeUserPasswordCommand : ICommand
    {
        [CommandOption("id")]
        public Guid UserId { get; init; }

        [CommandOption("old")]
        public string? OldPassword { get; init; }

        [CommandOption("new")]
        public string? NewPassword { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public ChangeUserPasswordCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PatchAsync("user/change-password", JsonContent.Create(this), console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
