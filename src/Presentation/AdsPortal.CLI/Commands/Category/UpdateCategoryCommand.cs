namespace AdsPortal.CLI.Commands.Category
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category update")]
    public class UpdateCategoryCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        [CommandOption("name", 'n')]
        public string? Name { get; init; }

        [CommandOption("description", 'd')]
        public string? Description { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public UpdateCategoryCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PutAsJsonAsync("category/update", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
