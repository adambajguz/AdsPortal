namespace AdsPortal.CLI.Application.Commands
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Modes;

    [Command("exit", Description = "Exits.",
             SupportedModes = new[] { typeof(InteractiveMode) })]
    public class ExitCommand : ICommand
    {
        private readonly ICliApplicationLifetime _cliApplicationLifetime;

        public ExitCommand(ICliApplicationLifetime cliApplicationLifetime)
        {
            _cliApplicationLifetime = cliApplicationLifetime;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _cliApplicationLifetime.RequestStop();

            return default;
        }
    }
}
