namespace AdsPortal.CLI.Commands.User
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Services;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user token")]
    public class AuthenticationTokenStatusCommand : ICommand
    {
        private readonly AuthTokenHolder _authTokenHolder;

        public AuthenticationTokenStatusCommand(AuthTokenHolder authTokenHolder)
        {
            _authTokenHolder = authTokenHolder;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("TOKEN:"));
            console.Output.Write(' ');
            await console.Output.WriteLineAsync(_authTokenHolder.Token ?? string.Empty);

            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("LEASE:"));
            console.Output.Write(' ');
            await console.Output.WriteLineAsync(_authTokenHolder.Lease.ToString());

            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("VALID TO:"));
            console.Output.Write(' ');
            await console.Output.WriteLineAsync(_authTokenHolder.ValidTo.ToString() + " UTC");
        }
    }
}
