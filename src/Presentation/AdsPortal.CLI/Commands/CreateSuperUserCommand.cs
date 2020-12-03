namespace AdsPortal.CLI.Commands
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.UserOperations.Commands.CreateUser;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Domain.Jwt;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("user create-superuser")]
    public class CreateSuperUserCommand : ICommand
    {
        private readonly IWebHostRunnerService _webHostRunnerService;

        public CreateSuperUserCommand(IWebHostRunnerService webHostRunnerService)
        {
            _webHostRunnerService = webHostRunnerService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            using (IWebHost webHost = await _webHostRunnerService.StartAsync())
            {
                IServiceScopeFactory serviceScopeFactory = webHost.Services.GetRequiredService<IServiceScopeFactory>();

                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    try
                    {
                        object result = await mediator.Send(new CreateUserCommand
                        {
                            Email = "test0@test.pl",
                            Name = "test0",
                            Password = "test1234",
                            Role = Roles.User | Roles.Admin,
                            Address = string.Empty,
                            PhoneNumber = string.Empty,
                            Surname = string.Empty
                        });

                        string json = JsonConvert.SerializeObject(result);
                        console.Output.WriteLine(json);
                    }
                    catch (Exception ex)
                    {
                        string json = JsonConvert.SerializeObject(ex);
                        console.Output.WriteLine(json);
                    }
                }
            }
        }
    }
}
