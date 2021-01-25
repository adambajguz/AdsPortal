namespace AdsPortal.CLI.Application.Commands.Tools.Test
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using CliWrap;
    using CliWrap.Buffered;
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

        private readonly ICliCommandExecutor _cliCommandExecutor;

        public TestWebApiCommand(ICliCommandExecutor cliCommandExecutor)
        {
            _cliCommandExecutor = cliCommandExecutor;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _cliCommandExecutor.ExecuteCommandAsync(@$"tools database drop -c ""{ConnectionString}"" -n ""{DatabaseName}"""); //TODO add command builder

            string current = Directory.GetCurrentDirectory();
            console.Output.WriteLine("Current directory is: {0}", current);
            console.Output.WriteLine("Current directory is: {0}", Path.GetFullPath(Path.Combine(current, "../../../../../../")));

            var buildResult = await Cli.Wrap("dotnet")
                                       .WithWorkingDirectory("../../../../../../WebApi/Presentation/AdsPortal.WebApi")
                                       .WithArguments(@$"build AdsPortal.WebApi.csproj -c Release -v m -o {current}/WebApi_Test")
                                       .WithEnvironmentVariables(e => e.Set("ASPNETCORE_ENVIRONMENT", "Test"))
                                       .WithValidation(CommandResultValidation.ZeroExitCode)
                                       .WithStandardOutputPipe(PipeTarget.ToStream(console.Output.BaseStream))
                                       .WithStandardErrorPipe(PipeTarget.ToStream(console.Error.BaseStream))
                                       .ExecuteBufferedAsync(console.GetCancellationToken());

            if (buildResult.ExitCode != 0)
            {
                console.Error.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"AdsPortal.WebApi failed to build."));
                return;
            }

            console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"AdsPortal.WebApi was build successfully."));

            CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(console.GetCancellationToken());

            Thread apiThread = new(async () => await StartApi(console, cancellationTokenSource.Token));
            apiThread.Start();

            await Task.Delay(8000);
            cancellationTokenSource.Cancel();

            await _cliCommandExecutor.ExecuteCommandAsync(@$"tools database drop -c ""{ConnectionString}"" -n ""{DatabaseName}"""); //TODO add command builder
        }

        private async Task StartApi(IConsole console, CancellationToken cancellationToken)
        {
            try
            {
                var runResult = await Cli.Wrap("dotnet")
                                   .WithWorkingDirectory("WebApi_Test")
                                   .WithArguments($@"AdsPortal.WebApi.dll")
                                   .WithEnvironmentVariables(e => e.Set("ASPNETCORE_ENVIRONMENT", "Test"))
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
