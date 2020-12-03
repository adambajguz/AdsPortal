namespace AdsPortal.CLI.Commands.User
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user delete")]
    public class DeleteUserCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public DeleteUserCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.DeleteAsync($"user/delete/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
