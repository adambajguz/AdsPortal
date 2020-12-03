namespace AdsPortal.CLI.Commands.User
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Common.Extensions;
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
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public ChangeUserPasswordCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PatchAsJsonAsync("user/update", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
