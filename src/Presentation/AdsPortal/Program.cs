namespace AdsPortal
{
    using System.Threading.Tasks;
    using AdsPortal.CLI;
    using Typin;

    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            CliApplicationBuilder cliBuilder = new();

            cliBuilder.UseStartup<CliStartup>()
                      .ConfigureServices((services) => services.AddFoundationCLIServices())
                      .AddCommandsFromThisAssembly();

            return await cliBuilder.Build().RunAsync();
        }
    }
}