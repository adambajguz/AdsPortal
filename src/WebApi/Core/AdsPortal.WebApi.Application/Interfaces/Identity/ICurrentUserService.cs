namespace AdsPortal.WebApi.Application.Interfaces.Identity
{
    using System;
    using AdsPortal.WebApi.Domain.Jwt;

    public interface ICurrentUserService
    {
        public Guid? Id { get; }
        string? Email { get; }
        string? Name { get; }
        string? Surname { get; }

        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }

        bool HasRole(Roles role);
        string[] GetRoles();
    }
}