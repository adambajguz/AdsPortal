namespace AdsPortal.Application.Interfaces.Identity
{
    using System;
    using AdsPortal.Domain.Jwt;

    public interface ICurrentUserService
    {
        public Guid? UserId { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }

        bool HasRole(Roles role);
        string[] GetRoles();
    }
}