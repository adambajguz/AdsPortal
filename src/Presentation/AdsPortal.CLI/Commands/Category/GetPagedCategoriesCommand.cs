﻿namespace AdsPortal.CLI.Commands.Category
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("category paged")]
    public class GetPagedCategoriesCommand : ICommand
    {
        [CommandOption("page", 'p', IsRequired = true)]
        public int Page { get; init; }

        [CommandOption("per-page", 'n', IsRequired = true)]
        public int EntiresPerPage { get; init; }

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public GetPagedCategoriesCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"category/get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
