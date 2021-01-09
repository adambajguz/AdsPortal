namespace AdsPortal.WebApi.Application.Interfaces.Identity
{
    using System;
    using AdsPortal.WebApi.Application.Operations.UserOperations.Queries.AuthenticateUser;
    using AdsPortal.WebApi.Domain.Jwt;

    public interface IJwtService
    {
        AuthenticateUserResponse GenerateJwtToken(IJwtUserData user);
        AuthenticateUserResponse GenerateJwtToken(IJwtUserData user, Roles roles);

        void ValidateStringToken(string? token);
        bool IsTokenStringValid(string? token);

        Guid GetUserIdFromToken(string token);
        bool IsRoleInToken(string? token, Roles role);
        bool IsAnyOfRolesInToken(string? token, Roles roles);
    }
}
