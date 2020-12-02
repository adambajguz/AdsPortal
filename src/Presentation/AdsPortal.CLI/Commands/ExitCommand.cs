namespace AdsPortal.CLI.Commands
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;

    [Command("exit", Description = "Exits from interactive mode.")]
    public class ExitCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;

        public ExitCommand(ICliRuntimeService cliRuntimeService)
        {
            _cliRuntimeService = cliRuntimeService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            await _cliRuntimeService.ExitAsync();
        }
    }
}
