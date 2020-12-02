namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Exceptions;
    using Typin.Modes;

    [Command("webhost restart",
             Description = "Restarts the webhost worker in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostRestartCommand : ICommand
    {
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostRestartCommand(IBackgroundWebHostProviderService webHostProviderService)
        {
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            if (_webHostProviderService.Status == WebHostStatuses.Stopped)
                throw new CommandException("WebHost is stopped.");

            await _webHostProviderService.RestartAsync(console.GetCancellationToken());
        }
    }
}
