namespace AdsPortal.CLI.Application.Commands.Advertisement
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("ad delete", Description = "Delete advertisement.")]
    public class DeleteAdvertisementCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteAdvertisementCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.DeleteAsync($"advertisement/delete/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
