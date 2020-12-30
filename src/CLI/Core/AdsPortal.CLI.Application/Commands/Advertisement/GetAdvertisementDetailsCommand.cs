namespace AdsPortal.CLI.Application.Commands.Advertisement
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ad get", Description = "Get advertisement details.")]
    public class GetAdvertisementDetailsCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public GetAdvertisementDetailsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"advertisement/get/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
