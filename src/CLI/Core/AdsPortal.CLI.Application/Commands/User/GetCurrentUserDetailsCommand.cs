namespace AdsPortal.CLI.Application.Commands.User
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user current")]
    public class GetCurrentUserDetailsCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetCurrentUserDetailsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"user/get-current", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
