namespace AdsPortal.CLI.Application.Commands.Category
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category delete", Description = "Delete category.")]
    public class DeleteCategoryCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteCategoryCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.DeleteAsync($"category/delete/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
