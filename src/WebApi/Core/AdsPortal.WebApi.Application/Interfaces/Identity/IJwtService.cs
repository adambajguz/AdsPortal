namespace AdsPortal.Application.Interfaces.Identity
{
    using System;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;
    using AdsPortal.WebApi.Domain.Jwt;

    public interface IJwtService
    {
        JwtTokenModel GenerateJwtToken(IJwtUserData user);
        JwtTokenModel GenerateJwtToken(IJwtUserData user, Roles roles);

        void ValidateStringToken(string? token);
        bool IsTokenStringValid(string? token);

        Guid GetUserIdFromToken(string token);
        bool IsRoleInToken(string? token, Roles role);
        bool IsAnyOfRolesInToken(string? token, Roles roles);
    }
}
