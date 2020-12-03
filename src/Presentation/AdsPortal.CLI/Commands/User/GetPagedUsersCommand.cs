namespace AdsPortal.CLI.Commands.User
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user paged")]
    public class GetPagedUsersCommand : ICommand
    {
        [CommandOption("page", 'p')]
        public int Page { get; init; }

        [CommandOption("per-page", 'n')]
        public int EntiresPerPage { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public GetPagedUsersCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"user/get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
