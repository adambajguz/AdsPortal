namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("webhost start",
             Description = "Starts the webhost worker in background in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostStartCommand : ICommand
    {
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostStartCommand(IBackgroundWebHostProviderService webHostProviderService)
        {
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostProviderService.StartAsync();
        }
    }
}
