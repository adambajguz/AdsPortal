namespace AdsPortal.Commands.Logging
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using CliFx;
    using CliFx.Attributes;

    [Command("logging get-level", Description = "Get console logging level in the interactive mode.")]
    public class GetLogLevelCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;

        public GetLogLevelCommand(ICliRuntimeService cliRuntimeService)
        {
            _cliRuntimeService = cliRuntimeService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            await console.Output.WriteLineAsync(SerilogConfiguration.ConsoleLoggingLevelSwitch.MinimumLevel.ToString());
        }
    }
}
