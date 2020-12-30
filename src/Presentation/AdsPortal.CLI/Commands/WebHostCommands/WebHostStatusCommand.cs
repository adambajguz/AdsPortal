namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Common;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("webhost status",
             Description = "Returns webhost worker status in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class WebHostStatusCommand : ICommand
    {
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public WebHostStatusCommand(IBackgroundWebHostProviderService webHostProviderService)
        {
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(AppInfo.AppName);
            await console.Output.WriteLineAsync($"Status: {_webHostProviderService.Status}");
            await console.Output.WriteLineAsync($"Startup time: {_webHostProviderService.StartupTime}");
            await console.Output.WriteLineAsync($"Runtime: {_webHostProviderService.Runtime}");
        }
    }
}
