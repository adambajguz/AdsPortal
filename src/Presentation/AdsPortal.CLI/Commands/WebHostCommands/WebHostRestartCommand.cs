namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;
    using CliFx.Exceptions;

    [Command("webhost restart", Description = "Restarts the webhost worker in the interactive mode.")]
    public class WebHostRestartCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostRestartCommand(ICliRuntimeService cliRuntimeService, IBackgroundWebHostProviderService webHostProviderService)
        {
            _cliRuntimeService = cliRuntimeService;
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            if (_webHostProviderService.Status == WebHostStatuses.Stopped)
                throw new CommandException("WebHost is stopped.");

            await _webHostProviderService.RestartAsync(console.GetCancellationToken());
        }
    }
}
