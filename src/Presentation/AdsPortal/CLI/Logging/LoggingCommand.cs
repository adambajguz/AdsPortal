namespace AdsPortal.CLI.Logging
{
    using System.Threading.Tasks;
    using AdsPortal.Infrastructure.Logging.Configuration;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("logging",
             Description = "Management of console logging level in interactive mode.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class LoggingCommand : ICommand
    {
        public async ValueTask ExecuteAsync(IConsole console)
        {
            await console.Output.WriteLineAsync(SerilogConfiguration.ConsoleLoggingLevelSwitch.MinimumLevel.ToString());
        }
    }
}
