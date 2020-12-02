namespace AdsPortal.Services
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interactive;
    using AdsPortal.CLI.Interfaces;
    using CliFx.Exceptions;

    public class CliRuntimeService : ICliRuntimeService
    {
        private readonly Lazy<bool> isInteractive;
        public bool IsInteractive => isInteractive.Value;

        public CliRuntimeService()
        {
            isInteractive = new Lazy<bool>(() =>
            {
                string? value = Environment.GetEnvironmentVariable(InteractiveCli.ModeEnvironmentVariable);

                if (bool.TryParse(value, out bool v))
                    return v;

                return false;
            });
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public Task ExitAsync()
        {
            Environment.Exit(0);

            return Task.CompletedTask;
        }

        public void ValidateInteractiveAndThrow()
        {
            if (!IsInteractive)
                throw new CommandException("This commands runs only in interactive mode.");
        }
    }
}
