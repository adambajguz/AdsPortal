namespace AdsPortal.CLI.Commands
{
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.CLI.Interfaces;
    using CliFx;
    using CliFx.Attributes;
    using MediatR;
    using Newtonsoft.Json;

    [Command("test2")]
    public class Test2Command : ICommand
    {
        private readonly IBackgroundWebHostProviderService _webHostProviderService;

        public Test2Command(IBackgroundWebHostProviderService webHostProviderService)
        {
            _webHostProviderService = webHostProviderService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            IMediator mediator = _webHostProviderService.GetService<IMediator>();

            ListResult<GetUsersListResponse>? result = await mediator.Send(new GetUsersListQuery());
            string json = JsonConvert.SerializeObject(result);

            console.Output.WriteLine(json);
            console.Output.WriteLine("All done2");
        }
    }
}
