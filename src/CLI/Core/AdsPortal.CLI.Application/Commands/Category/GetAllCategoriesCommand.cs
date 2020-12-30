namespace AdsPortal.CLI.Application.Commands.Category
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category all", Description = "Get all categories.")]
    public class GetAllCategoriesCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllCategoriesCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"category/get-all", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
