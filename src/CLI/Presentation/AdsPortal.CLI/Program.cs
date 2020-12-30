namespace AdsPortal.CLI
{
    using System.Threading.Tasks;
    using Typin;

    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await new CliApplicationBuilder()
                .UseStartup<CliStartup>()
                .Build()
                .RunAsync();
        }
    }
}