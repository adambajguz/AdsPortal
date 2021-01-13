namespace AdsPortal.DTO.Tests
{
    using Xunit;
    using Xunit.Abstractions;

    public class UserDTOCoherenceTests : BaseTest
    {
        public UserDTOCoherenceTests(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void Authenticate()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Queries.AuthenticateUser.AuthenticateUserQuery,
                           CLI.Application.Commands.User.AuthenticateCommand>();

            EnsureCoherent<WebApi.Application.Operations.UserOperations.Queries.AuthenticateUser.AuthenticateUserResponse,
                           CLI.Application.Models.AuthenticateUserResponse>();
        }

        [Fact]
        public void Create()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Commands.CreateUser.CreateUserCommand,
                           CLI.Application.Commands.User.CreateUserCommand,
                           WebPortal.Models.User.CreateUser>();
        }

        [Fact]
        public void Update()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Commands.UpdateUser.UpdateUserCommand,
                           CLI.Application.Commands.User.UpdateUserCommand,
                           WebPortal.Models.User.UpdateUser>();
        }

        [Fact]
        public void ChangePassword()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Commands.ChangeUserPassword.ChangeUserPasswordCommand,
                           CLI.Application.Commands.User.ChangeUserPasswordCommand>();
        }

        [Fact]
        public void GetDetails()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Queries.GetUserDetails.GetUserDetailsResponse,
                           WebPortal.Models.User.UserDetails>();
        }

        [Fact]
        public void GetAll()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Queries.GetUsersList.GetUsersListQuery,
                           CLI.Application.Commands.User.GetAllUsersCommand,
                           WebPortal.Models.User.GetUsersList>();

            EnsureCoherent<MediatR.GenericOperations.Models.ListResult<WebApi.Application.Operations.UserOperations.Queries.GetUsersList.GetUsersListResponse>,
                           WebPortal.Models.Base.ListResult<WebPortal.Models.User.UsersListItem>>();
        }

        [Fact]
        public void GetPaged()
        {
            EnsureCoherent<WebApi.Application.Operations.UserOperations.Queries.GetUsersList.GetPagedUsersListQuery,
                           CLI.Application.Commands.User.GetPagedUsersCommand,
                           WebPortal.Models.User.GetPagedUsersList>();

            EnsureCoherent<MediatR.GenericOperations.Models.PagedListResult<WebApi.Application.Operations.UserOperations.Queries.GetUsersList.GetUsersListResponse>,
                           WebPortal.Models.Base.PagedListResult<WebPortal.Models.User.UsersListItem>>();
        }
    }
}
