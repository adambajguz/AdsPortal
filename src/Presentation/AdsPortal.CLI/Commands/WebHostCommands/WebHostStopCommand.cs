namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("webhost stop",
             Description = "Stops the webhost background worker in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostStopCommand : ICommand
    {
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostStopCommand(IBackgroundWebHostProviderService webHostProviderService)
        {
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostProviderService.StopAsync(console.GetCancellationToken());
        }
    }
}
