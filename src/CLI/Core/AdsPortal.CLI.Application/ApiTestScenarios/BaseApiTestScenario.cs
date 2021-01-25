namespace AdsPortal.CLI.Application.ApiTestScenarios
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.TestScenarios;
    using Typin.Console;

    public abstract class BaseApiTestScenario : BaseTestScenario
    {
        public override string Name => "Initial state scenario";

        private readonly HttpClient _httpClient;

        public BaseApiTestScenario(HttpClient httpClient, IConsole console) : base(console)
        {
            _httpClient = httpClient;
        }

        protected async Task WaitForApiOrThrow()
        {
            using (CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(30)))
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                bool isLoaded;
                do
                {
                    await Task.Delay(250);
                    isLoaded = await IsRunning(cancellationTokenSource.Token);
                }
                while (!isLoaded && !cancellationTokenSource.IsCancellationRequested);

                stopwatch.Stop();

                if (!isLoaded && cancellationTokenSource.IsCancellationRequested)
                {
                    throw new ApplicationException("AdsPortal.WebApi not started.");
                }
            }
        }

        protected async Task<bool> IsRunning(CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:5001/health", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    if (await response.Content.ReadAsStringAsync() == "Healthy")
                    {
                        return true;
                    }

                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
