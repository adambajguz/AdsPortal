namespace AdsPortal.CLI.Commands
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    [Command("test")]
    public class TestCommand : ICommand
    {
        private readonly IWebHostRunnerService _webHostRunnerService;

        public TestCommand(IWebHostRunnerService webHostRunnerService)
        {
            _webHostRunnerService = webHostRunnerService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            using (IWebHost webHost = await _webHostRunnerService.StartAsync())
            {
                console.Output.WriteLine("Hello from test!");

                IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService<IServiceScopeFactory>();

                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    ListResult<GetUsersListResponse>? result = await mediator.Send(new GetUsersListQuery());
                    string json = JsonConvert.SerializeObject(result);

                    console.Output.WriteLine(json);
                    console.Output.WriteLine("All done");
                }
            }
        }
    }
}
