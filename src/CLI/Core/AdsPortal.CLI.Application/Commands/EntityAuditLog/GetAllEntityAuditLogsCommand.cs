namespace AdsPortal.CLI.Application.Commands.EntityAuditLog
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("entity-log all")]
    public class GetAllEntityAuditLogsCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllEntityAuditLogsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"entity-audit-log/get-all", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
