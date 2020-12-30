namespace AdsPortal.CLI.Application.Commands.User
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user delete")]
    public class DeleteUserCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteUserCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.DeleteAsync($"user/delete/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
