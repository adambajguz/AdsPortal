namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;

    [Command("webhost stop", Description = "Stops the webhost background worker in the interactive mode.")]
    public class WebHostStopCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostStopCommand(ICliRuntimeService cliRuntimeService, IBackgroundWebHostProviderService webHostProviderService)
        {
            _cliRuntimeService = cliRuntimeService;
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            await _webHostProviderService.StopAsync(console.GetCancellationToken());
        }
    }
}
