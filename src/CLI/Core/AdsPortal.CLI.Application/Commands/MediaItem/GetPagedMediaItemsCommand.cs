namespace AdsPortal.CLI.Application.Commands.MediaItem
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("media paged")]
    public class GetPagedMediaItemsCommand : ICommand
    {
        [CommandOption("page", 'p', IsRequired = true)]
        public int Page { get; init; }

        [CommandOption("per-page", 'n', IsRequired = true)]
        public int EntiresPerPage { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public GetPagedMediaItemsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"media/get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
