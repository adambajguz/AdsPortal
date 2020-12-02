namespace AdsPortal.CLI.Interactive
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CliFx;

    public static class CliApplicationBuilderExtensions
    {
        public static InteractiveCli BuildInteractive(this CliApplicationBuilder builder, string? interactiveStartupCommand = null, string? startupMessage = null, string? interactiveStartupMessage = null)
        {
            CliApplication cliApplication = builder.Build();
            return new InteractiveCli(cliApplication, interactiveStartupCommand, startupMessage, interactiveStartupMessage);
        }
    }

    public sealed class InteractiveCli
    {
        public const string ModeEnvironmentVariable = "INTERACTIVE_CLI_MODE";

        private readonly CliApplication _cliApplication;
        private readonly string? _interactiveStartupCommand;
        private readonly string? _startupMessage;
        private readonly string? _interactivestartupMessage;

        public ConsoleColor PromptForeground { get; set; } = ConsoleColor.Magenta;
        public ConsoleColor CommandForeground { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor FinishedResultForeground { get; set; } = ConsoleColor.White;

        public InteractiveCli(CliApplication cliApplication, string? interactiveStartupCommand = null, string? startupMessage = null, string? interactiveStartupMessage = null)
        {
            _cliApplication = cliApplication;
            _interactiveStartupCommand = interactiveStartupCommand;
            _startupMessage = startupMessage;
            _interactivestartupMessage = interactiveStartupMessage;
        }

        public async Task<int> RunAsync(bool alwaysStartInInteractive = false)
        {
            bool isInteractive = IsInteractive(alwaysStartInInteractive);
            Environment.SetEnvironmentVariable(ModeEnvironmentVariable, isInteractive.ToString());

            if (isInteractive)
                Console.Clear();

            // run interactive startup command
            if (_interactiveStartupCommand != null && isInteractive)
                await _cliApplication.RunAsync(_interactiveStartupCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            // print startup message if available
            if (isInteractive && _interactivestartupMessage != null)
            {
                PrintStartupMessage(_interactivestartupMessage);
            }
            else if (!isInteractive && _startupMessage != null)
            {
                PrintStartupMessage(_startupMessage);
            }

            // run interactively
            if (isInteractive)
            {
                string executableName = TryGetDefaultExecutableName() ?? string.Empty;
                ImmutableList<string> args = GetInitialArgs();

                while (true)
                {
                    if (args.Count == 0) // check if we already have some initial commands from arguments
                        args = GetInput(executableName);

                    int exitCode = await _cliApplication.RunAsync(args);
                    args = ImmutableList<string>.Empty;

                    ConsoleColor currentForeground = Console.ForegroundColor;
                    Console.ForegroundColor = FinishedResultForeground;

                    if (exitCode == 0)
                        Console.WriteLine($"{executableName}: Command finished succesfully");
                    else
                        Console.WriteLine($"{executableName}: Command finished with exit code ({exitCode})");

                    Console.ForegroundColor = currentForeground;
                }
            }

            //run normally
            return await _cliApplication.RunAsync();
        }

        private void PrintStartupMessage(string message)
        {
            ConsoleColor currentForeground = Console.ForegroundColor == PromptForeground ? Console.ForegroundColor : ConsoleColor.Gray;
            Console.ForegroundColor = PromptForeground;
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ForegroundColor = currentForeground;
        }

        private ImmutableList<string> GetInitialArgs()
        {
            IEnumerable<string> args = Environment.GetCommandLineArgs().Skip(1);

            if (args.FirstOrDefault() == Directives.Interactive)
                return args.Skip(1).ToImmutableList();

            return args.ToImmutableList();
        }

        private ImmutableList<string> GetInput(string executableName)
        {
            ImmutableList<string>? arguments;
            string line;
            do
            {
                ConsoleColor currentForeground = Console.ForegroundColor == PromptForeground ? Console.ForegroundColor : ConsoleColor.Gray;
                Console.ForegroundColor = PromptForeground;
                Console.Write(executableName);
                Console.Write("> ");
                Console.ForegroundColor = CommandForeground;

                line = Console.ReadLine();
                Console.ForegroundColor = currentForeground;

                if (line.ToLower() == Directives.Default)
                    return ImmutableList<string>.Empty;

                arguments = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToImmutableList();

            } while (string.IsNullOrWhiteSpace(line));

            return arguments;
        }

        private static bool IsInteractive(bool alwaysStartInInteractive)
        {
            if (alwaysStartInInteractive)
                return true;

            string? arg = Environment.GetCommandLineArgs()
                                     .Skip(1)
                                     .FirstOrDefault();

            return arg != null && arg.ToLower() == Directives.Interactive;
        }

        private static string? TryGetDefaultExecutableName()
        {
            string entryAssemblyLocation = Environment.GetCommandLineArgs().First();

            // The assembly can be an executable or a dll, depending on how it was packaged
            bool isDll = string.Equals(Path.GetExtension(entryAssemblyLocation), ".dll", StringComparison.OrdinalIgnoreCase);

            return isDll
                ? "dotnet " + Path.GetFileName(entryAssemblyLocation)
                : Path.GetFileNameWithoutExtension(entryAssemblyLocation);
        }
    }
}
