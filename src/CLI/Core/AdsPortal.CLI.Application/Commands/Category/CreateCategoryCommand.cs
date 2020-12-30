namespace AdsPortal.CLI.Application.Commands.Category
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category create", Description = "Create new category.")]
    public class CreateCategoryCommand : ICommand
    {
        [CommandOption("name", 'n')]
        public string? Name { get; init; }

        [CommandOption("description", 'd')]
        public string? Description { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public CreateCategoryCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("category/create", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
