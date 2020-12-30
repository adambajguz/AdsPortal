namespace AdsPortal.CLI.Application.Commands.Advertisement
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ad paged", Description = "Get paged advertisements.")]
    public class GetPagedAdvertisementsCommand : ICommand
    {
        [CommandOption("page", 'p', IsRequired = true)]
        public int Page { get; init; }

        [CommandOption("per-page", 'n', IsRequired = true)]
        public int EntiresPerPage { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public GetPagedAdvertisementsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"advertisement/get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
