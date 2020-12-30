namespace AdsPortal.CLI.Application.Commands.EntityAuditLog
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Helpers;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("entity-log get")]
    public class GetEntityAuditLogDetailsCommand : ICommand
    {
        [CommandOption("id")]
        public Guid Id { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;

        public GetEntityAuditLogDetailsCommand(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"entity-audit-log/get/{Id}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
