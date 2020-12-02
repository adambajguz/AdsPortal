namespace AdsPortal.ManagementUI.Services.Data
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Application.Exceptions;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Operations.UserOperations.Commands.ChangePassword;
    using AdsPortal.Application.Operations.UserOperations.Commands.CreateUser;
    using AdsPortal.Application.Operations.UserOperations.Commands.DeleteUser;
    using AdsPortal.Application.Operations.UserOperations.Commands.UpdateUser;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUserDetails;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList;
    using AdsPortal.Application.OperationsModels.Core;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class UserService
    {
        private IMediator Mediator { get; }

        public UserService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public async Task<IdResult> CreateUser(CreateUserCommand user)
        {
            return await Mediator.Send(user);
        }

        public async Task<GetUserDetailsResponse> GetCurrentUserDetails([FromServices] ICurrentUserService currentUser)
        {
            Guid id = currentUser.UserId ?? throw new ForbiddenException();

            return await Mediator.Send(new GetUserDetailsQuery(id));
        }

        public async Task<GetUserDetailsResponse> GetUserDetails(Guid id)
        {
            return await Mediator.Send(new GetUserDetailsQuery(id));
        }

        public async Task<Unit> UpdateUser(UpdateUserCommand user)
        {
            return await Mediator.Send(user);
        }

        public async Task<Unit> DeleteUser(Guid id)
        {
            return await Mediator.Send(new DeleteUserCommand(id));
        }

        public async Task<Unit> ChangeUserPassword(ChangePasswordCommand request)
        {
            return await Mediator.Send(request);
        }

        public async Task<ListResult<GetUsersListResponse>> GetUsersList()
        {
            return await Mediator.Send(new GetUsersListQuery());
        }
    }
}
