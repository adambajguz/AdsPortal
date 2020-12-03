namespace AdsPortal.CLI.Helpers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Typin.Console;

    public static class HttpClientHelpers
    {
        public static async Task PrintResponse(this HttpResponseMessage response, IConsole console)
        {
            console.Output.WriteLine();

            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("REQUEST URL:"));
            console.Output.Write(' ');
            await console.Output.WriteLineAsync(response.RequestMessage?.RequestUri?.ToString() ?? string.Empty);

            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("REQUEST HEADERS:"));
            console.Output.Write(' ');
            console.Output.WriteLine();
            await console.Output.WriteLineAsync(response.RequestMessage?.Headers?.ToString() ?? string.Empty);

            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("RESPONSE:"));
            console.Output.Write(' ');
            console.Output.WriteLine();
            await console.Output.WriteLineAsync(response.ToString());

            console.Output.WriteLine();
            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("RESPONSE HEADERS:"));
            console.Output.Write(' ');
            console.Output.WriteLine();
            await console.Output.WriteLineAsync(response.Headers.ToString());

            console.Output.WriteLine();
            console.WithColors(ConsoleColor.Black, ConsoleColor.White, () => console.Output.Write("RESPONSE PAYLOAD:"));
            console.Output.Write(' ');
            console.Output.WriteLine();

            string responseString = await response.Content.ReadAsStringAsync(console.GetCancellationToken());
            await console.Output.WriteLineAsync(responseString);
        }
    }
}
