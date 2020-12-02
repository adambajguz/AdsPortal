namespace AdsPortal.Commands.Logging
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using CliFx;
    using CliFx.Attributes;

    [Command("logging", Description = "Management of console logging level in interactive mode.")]
    public class LoggingCommand : ICommand
    {
        private readonly ICliRuntimeService _cliRuntimeService;

        public LoggingCommand(ICliRuntimeService cliRuntimeService)
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
