namespace AdsPortal.CLI.Application.Commands.Advertisement
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ad create", Description = "Create new advertisement.")]
    public class CreateAdvertisementCommand : ICommand
    {
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

        public CreateAdvertisementCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("advertisement/create", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
