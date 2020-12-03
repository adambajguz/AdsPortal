namespace AdsPortal.CLI.Commands.User
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user token clear")]
    public class AuthenticationClearStatusCommand : ICommand
    {
        private readonly AuthTokenHolder _authTokenHolder;

        public AuthenticationClearStatusCommand(AuthTokenHolder authTokenHolder)
        {
            _authTokenHolder = authTokenHolder;
        }

        public ValueTask ExecuteAsync(IConsole console)
        {
            _authTokenHolder.Clear();

            return default;
        }
    }
}
