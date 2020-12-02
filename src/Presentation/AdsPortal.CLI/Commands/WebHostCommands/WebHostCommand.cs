namespace AdsPortal.CLI.Commands.WebHostCommands
{
    using System.Threading.Tasks;
    using CliFx;
    using CliFx.Attributes;
    using CliFx.Exceptions;

    [Command("webhost", Description = "Management of the background webhost in the interactive mode.")]
    public class WebHostCommand : ICommand
    {
        public WebHostCommand()
        {

        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            throw new CommandException(exitCode: 0, showHelp: true);
        }
    }
}
