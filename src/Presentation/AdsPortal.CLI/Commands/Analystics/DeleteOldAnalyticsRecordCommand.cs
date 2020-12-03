namespace AdsPortal.CLI.Commands.Analystics
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("analytics delete")]
    public class DeleteOldAnalyticsRecordCommand : ICommand
    {
        [CommandOption("date")]
        public DateTime? OlderThanOrEqualToDate { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public DeleteOldAnalyticsRecordCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.DeleteAsync($"analytics-record/delete-old", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
