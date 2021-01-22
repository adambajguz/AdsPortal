namespace AdsPortal.CLI.Application.Commands.Tools.Test
{
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using CliWrap;
    using CliWrap.Buffered;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("tools test WebApi", Description = "Tests 'AdsPortal.WebApi'.")]
    public class TestWebApiCommand : ICommand
    {
        [CommandOption("conn-string", 'c', Description = "Connection string.", FallbackVariableName = "CONNECTION_STRING")]
        public string ConnectionString { get; init; } = @"Data Source=.\SQLEXPRESS;Integrated Security=True;Database=master";

        [CommandOption("name", 'n', Description = "Database name.", FallbackVariableName = "DB_NAME")]
        public string DatabaseName { get; init; } = "AdsPortal_Test";

        private readonly ICliApplicationLifetime _cliApplicationLifetime;
        private readonly ICliCommandExecutor _cliCommandExecutor;

        public TestWebApiCommand(ICliApplicationLifetime cliApplicationLifetime, ICliCommandExecutor cliCommandExecutor)
        {
            _cliApplicationLifetime = cliApplicationLifetime;
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
                                       .ExecuteBufferedAsync();

            Process myProcess = new Process();

            myProcess.StartInfo.UseShellExecute = true;
            // You can start any process, HelloWorld is a do-nothing example.
            myProcess.StartInfo.FileName = $"{current}\\WebApi_Test\\AdsPortal.WebApi.exe";
            myProcess.Start();
            // This code assumes the process you are starting will terminate itself.
            // Given that is is started without a window so you cannot terminate it
            // on the desktop, it must terminate itself or you can do it programmatically
            // from this application using the Kill method.


            //using (Process process = new Process())
            //{
            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.UseShellExecute = false;
            //    process.StartInfo.FileName = $"{current}\\WebApi_Test\\AdsPortal.WebApi.exe";
            //    process.StartInfo.EnvironmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Test");
            //    process.Start();

            //await Task.Delay(10000);

            //    process.Close();
            //}

            await _cliCommandExecutor.ExecuteCommandAsync(@$"tools database drop -c ""{ConnectionString}"" -n ""{DatabaseName}"""); //TODO add command builder
            //_cliApplicationLifetime.RequestStop();
        }
    }
}
