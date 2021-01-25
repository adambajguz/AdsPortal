namespace AdsPortal.CLI.Application.Commands.Tools.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.CLI.Application.TestScenarios;
    using CliWrap;
    using CliWrap.Buffered;
    using FluentAssertions.Execution;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("tools test WebApi", Description = "Tests 'AdsPortal.WebApi'.")]
    public class TestWebApiCommand : ICommand
    {
        [CommandOption("conn-string", 'c', Description = "Connection string.", FallbackVariableName = "CONNECTION_STRING")]
        public string ConnectionString { get; init; } = @"Data Source=.\SQLEXPRESS;Integrated Security=True;Database=master";

        [CommandOption("name", 'n', Description = "Database name.", FallbackVariableName = "DB_NAME")]
        public string DatabaseName { get; init; } = "AdsPortal_Test";

        [CommandOption("no-build", Description = "Do not build app.")]
        public bool NoBuild { get; init; }

        private readonly ICliCommandExecutor _cliCommandExecutor;
        private readonly IEnumerable<ITestScenario> _testScenarios;

        public TestWebApiCommand(ICliCommandExecutor cliCommandExecutor, IEnumerable<ITestScenario> testScenarios)
        {
            _cliCommandExecutor = cliCommandExecutor;
            _testScenarios = testScenarios;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _cliCommandExecutor.ExecuteCommandAsync(@$"tools database drop -c ""{ConnectionString}"" -n ""{DatabaseName}"""); //TODO add command builder

            if (!_testScenarios.Any())
            {
                console.Error.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"No test scenarios in the app."));
                return;
            }

            CancellationToken cancellationToken = console.GetCancellationToken();

            string current = Directory.GetCurrentDirectory();
            console.Output.WriteLine("Current directory is: {0}", current);
            console.Output.WriteLine("Current directory is: {0}", Path.GetFullPath(Path.Combine(current, "../../../../../../")));

            if (!NoBuild)
            {
                BufferedCommandResult buildResult = await Cli.Wrap("dotnet")
                                                             .WithWorkingDirectory("../../../../../../WebApi/Presentation/AdsPortal.WebApi")
                                                             .WithArguments(@$"build AdsPortal.WebApi.csproj -c Release -v m -o {current}/WebApi_Test")
                                                             .WithEnvironmentVariables(e => e.Set("ASPNETCORE_ENVIRONMENT", "Test"))
                                                             .WithValidation(CommandResultValidation.ZeroExitCode)
                                                             .WithStandardOutputPipe(PipeTarget.ToStream(console.Output.BaseStream))
                                                             .WithStandardErrorPipe(PipeTarget.ToStream(console.Error.BaseStream))
                                                             .ExecuteBufferedAsync(cancellationToken);

                if (buildResult.ExitCode != 0)
                {
                    console.Error.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"AdsPortal.WebApi failed to build."));
                    return;
                }

                console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"AdsPortal.WebApi was build successfully."));
                console.Output.WriteLine();
            }

            int count = _testScenarios.Count();
            int currentId = 0;

            console.Output.WithForegroundColor(ConsoleColor.DarkGray, (o) => o.WriteLine($"Executing {count} scenarios..."));

            foreach (ITestScenario scenario in _testScenarios)
            {
                ++currentId;

                console.Output.WithForegroundColor(ConsoleColor.Cyan, (o) => o.WriteLine($"Running test scenario {currentId}/{count} '{scenario.Name}' ({scenario.GetType().FullName})..."));
                console.Output.WithForegroundColor(ConsoleColor.DarkGray, (o) => o.WriteLine($"Starting AdsPortal.WebApi..."));

                CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                Thread apiThread = new(async () => await StartApi(console, cancellationTokenSource.Token));
                apiThread.Start();

                await Task.Delay(15000);
                console.Output.WithForegroundColor(ConsoleColor.DarkGray, (o) => o.WriteLine($"Executing scenario..."));

                try
                {
                    await scenario.ExecuteAsync(cancellationToken);
                    console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"Successfully finished test scenario {currentId}/{count}..."));
                }
                catch (AssertionFailedException ex)
                {
                    console.Output.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"Test scenario {currentId}/{count} assertion failed."));
                    ExceptionFormatter.WriteException(console.Error, ex);

                    return;
                }
                catch (Exception ex)
                {
                    console.Output.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"Test scenario {currentId}/{count} failed to execute."));
                    ExceptionFormatter.WriteException(console.Error, ex);

                    return;
                }
                finally
                {
                    cancellationTokenSource.Cancel();
                    await _cliCommandExecutor.ExecuteCommandAsync(@$"tools database drop -c ""{ConnectionString}"" -n ""{DatabaseName}"""); //TODO add command builder

                    console.Output.WithForegroundColor(ConsoleColor.DarkGray, (o) => o.WriteLine($"Disposed test scenario {currentId}/{count}..."));
                    console.Output.WriteLine();
                }
            }
        }

        private static async Task StartApi(IConsole console, CancellationToken cancellationToken)
        {
            try
            {
                BufferedCommandResult runResult = await Cli.Wrap("dotnet")
                                                           .WithWorkingDirectory("WebApi_Test")
                                                           .WithArguments($@"AdsPortal.WebApi.dll")
                                                           .WithEnvironmentVariables(e => e.Set("ASPNETCORE_ENVIRONMENT", "Test"))
                                                           //.WithEnvironmentVariables(e => e.Set("ASPNETCORE_URLS", "https://+:5001"))
                                                           .WithValidation(CommandResultValidation.ZeroExitCode)
                                                           .WithStandardOutputPipe(PipeTarget.ToStream(console.Output.BaseStream))
                                                           .WithStandardErrorPipe(PipeTarget.ToStream(console.Error.BaseStream))
                                                           .ExecuteBufferedAsync(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"AdsPortal.WebApi closed."));
            }
            catch (Exception ex)
            {
                console.Error.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"Fatal error occured in AdsPortal.WebApi."));
                ExceptionFormatter.WriteException(console.Error, ex);
            }
        }
    }
}
