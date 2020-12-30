namespace AdsPortal.CLI.Application.Commands.MediaItem
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("media all")]
    public class GetAllMediaItemsCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllMediaItemsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"media/get-all", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
