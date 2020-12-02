namespace AdsPortal.CLI.Logging
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using Serilog.Events;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("logging set-level",
             Description = "Set console logging level in the interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class SetLogLevelCommand : ICommand
    {
        [CommandParameter(0, Description = "Console logging level")]
        public LogEventLevel Value { get; set; }

        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public SetLogLevelCommand(IBackgroundWebHostProviderService webHostProviderService)
        {
            _webHostProviderService = webHostProviderService;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            SerilogConfiguration.ConsoleLoggingLevelSwitch.MinimumLevel = Value;

            return default;
        }
    }
}
