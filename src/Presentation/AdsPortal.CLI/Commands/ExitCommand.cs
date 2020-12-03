namespace AdsPortal.CLI.Commands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("exit", Description = "Exits.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class ExitCommand : ICommand
    {
        private readonly ICliApplicationLifetime _cliApplicationLifetime;
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public ExitCommand(ICliApplicationLifetime cliApplicationLifetime, IBackgroundWebHostProviderService webHostProviderService)
        {
            _cliApplicationLifetime = cliApplicationLifetime;
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostProviderService.StopAsync(console.GetCancellationToken());
            _cliApplicationLifetime.RequestStop();
        }
    }
}
