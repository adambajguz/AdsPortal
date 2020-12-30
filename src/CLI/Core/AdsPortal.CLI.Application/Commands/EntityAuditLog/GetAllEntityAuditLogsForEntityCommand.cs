namespace AdsPortal.CLI.Application.Commands.EntityAuditLog
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("entity-log all-for-entity")]
    public class GetAllEntityAuditLogsForEntityCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public GetAllEntityAuditLogsForEntityCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"get-all-for-entity/get-all/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
