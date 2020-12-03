﻿namespace AdsPortal.CLI.Commands.User
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Helpers;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user all")]
    public class GetAllUsersCommand : ICommand
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundWebHostProviderService _backgroundWebHostProvider;

        public GetAllUsersCommand(IHttpClientFactory httpClientFactory, IBackgroundWebHostProviderService backgroundWebHostProvider)
        {
            _httpClientFactory = httpClientFactory;
            _backgroundWebHostProvider = backgroundWebHostProvider;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _backgroundWebHostProvider.StartAsync(console.GetCancellationToken());

            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.GetAsync($"user/get-all", console.GetCancellationToken());

            await response.PrintResponse(console);
        }
    }
}
