namespace AdsPortal.CLI.Commands
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Operations.UserOperations.Commands.CreateUser;
    using AdsPortal.CLI.Interfaces;
    using AdsPortal.Domain.Jwt;
    using CliFx;
    using CliFx.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

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
                console.Output.WriteLine("Hello from test!");

                IServiceScopeFactory serviceScopeFactory = webHost.Services.GetService<IServiceScopeFactory>();

                //replace with api call using httpclient
                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    HttpContext httpContext = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                    //IJwtService jwt = scope.ServiceProvider.GetRequiredService<IJwtService>();

                    //JwtTokenModel tokenModel = jwt.GenerateJwtToken(new User
                    //{
                    //    Id = Guid.NewGuid(),
                    //    CreatedOn = DateTime.UtcNow,
                    //    CreatedBy = null,
                    //    Email = "superadmin@adsportal.com",
                    //    Role = Roles.User | Roles.Editor | Roles.Admin,
                    //    IsActive = true
                    //});

                    try
                    {
                        object result = await mediator.Send(new CreateUserCommand
                        {
                            Email = "test0@test.pl",
                            Name = "test0",
                            Password = "test1234",
                            Role = Roles.User | Roles.Editor | Roles.Admin,
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
