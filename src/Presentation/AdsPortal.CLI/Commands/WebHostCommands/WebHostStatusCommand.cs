namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Common;
    using CliFx;
    using CliFx.Attributes;

    [Command("webhost status", Description = "Returns webhost worker status in the interactive mode.")]
    public class WebHostStatusCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostStatusCommand(ICliRuntimeService cliRuntimeService, IBackgroundWebHostProviderService webHostProviderService)
        {
            _cliRuntimeService = cliRuntimeService;
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            await console.Output.WriteLineAsync(GlobalAppConfig.AppInfo.AppName);
            await console.Output.WriteLineAsync($"Status: {_webHostProviderService.Status}");
            await console.Output.WriteLineAsync($"Startup time: {_webHostProviderService.StartupTime}");
            await console.Output.WriteLineAsync($"Runtime: {_webHostProviderService.Runtime}");
        }
    }
}
