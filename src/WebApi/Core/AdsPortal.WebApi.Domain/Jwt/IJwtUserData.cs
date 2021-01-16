namespace AdsPortal.WebApi.Domain.Jwt
{
    using System;

    public interface IJwtUserData
    {
        Guid Id { get; }

        DateTime CreatedOn { get; }
        Guid? CreatedBy { get; }

        string Email { get; }
        string Name { get; }
        string Surname { get; }

        Roles Role { get; }
        bool IsActive { get; }
    }
}
