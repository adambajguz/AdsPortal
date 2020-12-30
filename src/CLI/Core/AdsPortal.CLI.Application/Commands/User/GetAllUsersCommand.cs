namespace AdsPortal.CLI.Application.Commands.User
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user all")]
    public class GetAllUsersCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllUsersCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"user/get-all", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
