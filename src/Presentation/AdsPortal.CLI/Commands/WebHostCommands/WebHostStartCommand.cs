namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;

    [Command("webhost start", Description = "Starts the webhost worker in background in the interactive mode.")]
    public class WebHostStartCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostStartCommand(ICliRuntimeService cliRuntimeService, IBackgroundWebHostProviderService webHostProviderService)
        {
            _cliRuntimeService = cliRuntimeService;
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            await _webHostProviderService.StartAsync();
        }
    }
}
