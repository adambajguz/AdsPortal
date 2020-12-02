namespace AdsPortal.Commands.Logging
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using CliFx;
    using CliFx.Attributes;
    using Serilog.Events;

    [Command("logging set-level", Description = "Set console logging level in the interactive mode.")]
    public class SetLogLevelCommand : ICommand
    {
        [CommandParameter(0, Description = "Console logging level")]
        public LogEventLevel Value { get; set; }

        private readonly ICliRuntimeService _cliRuntimeService;
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public SetLogLevelCommand(ICliRuntimeService cliRuntimeService, IBackgroundWebHostProviderService webHostProviderService)
        {
            _cliRuntimeService = cliRuntimeService;
            _webHostProviderService = webHostProviderService;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _cliRuntimeService.ValidateInteractiveAndThrow();

            SerilogConfiguration.ConsoleLoggingLevelSwitch.MinimumLevel = Value;

            return default;
        }
    }
}
