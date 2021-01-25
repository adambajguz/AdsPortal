namespace AdsPortal.CLI.Application.ApiTestScenarios
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Client;
    using FluentAssertions;
    using Typin.Console;

    public sealed class InitialStateTestScenario : BaseApiTestScenario
    {
        public override string Name => "Initial state scenario";

        private readonly WebApiClientAggregator _webApi;

        public InitialStateTestScenario(WebApiClientAggregator webApi, HttpClient httpClient, IConsole console) : base(httpClient, console)
        {
            _webApi = webApi;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await WaitForApiOrThrow();

            var response = await _webApi.UserClient.AuthAsync(new AuthenticateUserQuery
            {
                Email = "admin@adsportal.com",
                Password = "Pass123$"
            }, cancellationToken);

            _webApi.SetToken(response.Token);

            true.Should().BeFalse();
        }
    }
}
