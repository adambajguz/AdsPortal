namespace AdsPortal.CLI.Exceptions
{
    using System;
    using System.Net.Http;
    using Typin.Console;
    using Typin.Exceptions;

    public class HttpExceptionHandler : ICliExceptionHandler
    {
        private readonly IConsole _console;

        public HttpExceptionHandler(IConsole console)
        {
            _console = console;
        }

        public bool HandleException(Exception ex)
        {
            IConsole console = _console;

            switch (ex)
            {
                // Swallow directive exceptions and route them to the console
                case HttpRequestException hrex:
                    {
                        WriteError(console, hrex.Message);
                    }
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Write an error message to the console.
        /// </summary>
        private static void WriteError(IConsole console, string message)
        {
            console.Error.WithForegroundColor(ConsoleColor.Red, (error) => error.WriteLine(message));
            console.Error.WriteLine();
        }
    }
}

