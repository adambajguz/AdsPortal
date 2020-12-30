namespace AdsPortal.CLI.Application.Commands.Category
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category paged", Description = "Get paged categories.")]
    public class GetPagedCategoriesCommand : ICommand
    {
        [CommandOption("page", 'p', IsRequired = true)]
        public int Page { get; init; }

        [CommandOption("per-page", 'n', IsRequired = true)]
        public int EntiresPerPage { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public GetPagedCategoriesCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"category/get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
