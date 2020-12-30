namespace AdsPortal.CLI.Application.Commands.Advertisement
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ad all", Description = "Get all advertisements.")]
    public class GetAllAdvertisementsCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllAdvertisementsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"advertisement/get-all", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
