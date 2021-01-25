namespace AdsPortal.CLI.Application.ApiTestScenarios
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.Models;
    using AdsPortal.CLI.Application.TestScenarios;
    using FluentAssertions;
    using Typin.Console;

    public sealed class InitialStateTestScenario : ITestScenario
    {
        public string Name => "Initial state scenario";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConsole _console;

        public InitialStateTestScenario(IHttpClientFactory httpClientFactory, IConsole console)
        {
            _httpClientFactory = httpClientFactory;
            _console = console;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            HttpClient client = _httpClientFactory.CreateClient("api");
            var response = await client.PostAsJsonAsync("user/auth", this, _console.GetCancellationToken());

            AuthenticateUserResponse? model = await response.Content.ReadFromJsonAsync<AuthenticateUserResponse>();


            true.Should().BeFalse();
        }
    }
}
