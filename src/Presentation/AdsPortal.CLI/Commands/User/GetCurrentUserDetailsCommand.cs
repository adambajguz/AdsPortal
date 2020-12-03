namespace AdsPortal.CLI.Commands.User
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user current")]
    public class GetCurrentUserDetailsCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public GetCurrentUserDetailsCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"user/get-current", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
