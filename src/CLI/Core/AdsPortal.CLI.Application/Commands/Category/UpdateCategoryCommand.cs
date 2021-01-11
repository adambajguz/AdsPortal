namespace AdsPortal.CLI.Application.Commands.Category
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category update", Description = "Update category details.")]
    public class UpdateCategoryCommand : ICommand
    {
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        [CommandOption("id")]
        public Guid Id { get; init; }

        [CommandOption("name", 'n')]
        public string? Name { get; init; }

        [CommandOption("description", 'd')]
        public string? Description { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public UpdateCategoryCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PutAsJsonAsync($"category/update/{Id}", this, console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
