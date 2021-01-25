namespace AdsPortal.CLI.Application.ApiTestScenarios
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Client;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Typin.Console;

    public sealed class InitialStateTestScenario : BaseApiTestScenario
    {
        public override string Name => "Initial state scenario";

        private readonly WebApiClientAggregator _webApi;

        public InitialStateTestScenario(WebApiClientAggregator webApi, HttpClient httpClient, IConsole console, ILogger<InitialStateTestScenario> logger) : base(httpClient, console, logger)
        {
            _webApi = webApi;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            await WaitForApiOrThrow();

            await Test(Login, cancellationToken);
            await Test(ShouldReturnOneDefaultUser, cancellationToken);
            await Test(ShouldReturnNoCategories, cancellationToken);
            await Test(ShouldReturnNoAdvertisements, cancellationToken);
            await Test(ShouldReturnNoMediaItems, cancellationToken);

            await Test(ShouldAddCategory, cancellationToken);
            await Test(ShouldReturnOneCategory, cancellationToken);

            await Test(ShouldAddUser, cancellationToken);
            await Test(ShouldReturnTwoUsers, cancellationToken);
        }

        private async Task Login(CancellationToken cancellationToken)
        {
            AuthenticateUserResponse response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.UserClient.AuthAsync(new AuthenticateUserQuery
                {
                    Email = "admin@adsportal.com",
                    Password = "Pass123$"
                }, cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Token.Should().NotBeNullOrWhiteSpace();
            response.Token!.Length.Should().BeGreaterOrEqualTo(300);

            response.Lease.Should().NotBeNullOrWhiteSpace();
            response.Lease.Should().Be("1.00:00:00");

            response.ValidTo.Should().BeAfter(System.DateTimeOffset.UtcNow.AddHours(23));

            _webApi.SetToken(response.Token);
        }

        private async Task ShouldReturnOneDefaultUser(CancellationToken cancellationToken)
        {
            GetUsersListResponseListResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.UserClient.GetAll5Async(cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Count.Should().Be(1);
            response.Entries.Should().NotBeNull();
            response.Entries!.First().Name.Should().Be("Admin");
        }

        private async Task ShouldReturnNoCategories(CancellationToken cancellationToken)
        {
            GetCategoriesListResponseListResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.CategoryClient.GetAll2Async(cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Count.Should().Be(0);
            response.Entries.Should().NotBeNull();
        }

        private async Task ShouldReturnNoAdvertisements(CancellationToken cancellationToken)
        {
            GetAdvertisementsListResponseListResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.AdvertisementClient.GetAllAsync(cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Count.Should().Be(0);
            response.Entries.Should().NotBeNull();
        }

        private async Task ShouldReturnNoMediaItems(CancellationToken cancellationToken)
        {
            GetMediaItemsListResponseListResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.MediaItemClient.GetAll4Async(cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Count.Should().Be(0);
            response.Entries.Should().NotBeNull();
        }

        private async Task ShouldAddCategory(CancellationToken cancellationToken)
        {
            IdResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.CategoryClient.Create2Async(new CreateCategoryCommand
                {
                    Name = "name",
                    Description = "description"
                }, cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Should().NotBeNull();
            response.Id.Should().NotBeEmpty();
        }

        private async Task ShouldReturnOneCategory(CancellationToken cancellationToken)
        {
            GetCategoriesListResponseListResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.CategoryClient.GetAll2Async(cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Count.Should().Be(1);
            response.Entries.Should().NotBeNull();
        }

        private async Task ShouldAddUser(CancellationToken cancellationToken)
        {
            IdResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.UserClient.Create4Async(new CreateUserCommand
                {
                    Email = "name@domain.com",
                    Password = "test1234$XYZ",
                    Name = "newuser",
                    Role = Roles.User,
                    Surname = "newsurname"
                }, cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Should().NotBeNull();
            response.Id.Should().NotBeEmpty();
        }

        private async Task ShouldReturnTwoUsers(CancellationToken cancellationToken)
        {
            GetUsersListResponseListResult response = default!;

            Func<Task> act = async () =>
            {
                response = await _webApi.UserClient.GetAll5Async(cancellationToken);
            };

            await act.Should().NotThrowAsync();

            response.Count.Should().Be(2);
        }
    }
}
