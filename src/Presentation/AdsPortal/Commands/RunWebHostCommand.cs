namespace AdsPortal.Commands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;

    [Command(Description = "Runs webhost in normal mode.")]
    public class RunWebHostCommand : ICommand
    {
        private readonly IWebHostRunnerService _webHostRunnerService;

        public RunWebHostCommand(IWebHostRunnerService webHostRunnerService)
        {
            _webHostRunnerService = webHostRunnerService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _webHostRunnerService.RunAsync(console.GetCancellationToken());
        }
    }
}
