namespace AdsPortal.CLI.Commands.Advertisement
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ad update")]
    public class UpdateAdvertisementCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        [CommandOption("title", 't')]
        public string? Title { get; init; } = string.Empty;

        [CommandOption("description", 'd')]
        public string? Description { get; init; } = string.Empty;

        [CommandOption("published", 'p')]
        public bool IsPublished { get; init; }

        [CommandOption("visible-to", 'v')]
        public DateTime VisibleTo { get; init; }

        [CommandOption("iid")]
        public Guid? CoverImageId { get; init; }

        [CommandOption("cid")]
        public Guid CategoryId { get; init; }

        [CommandOption("aid")]
        public Guid AuthorId { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public UpdateAdvertisementCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PutAsJsonAsync("advertisement/update", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
