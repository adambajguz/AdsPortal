namespace AdsPortal.WebApi.Domain.Jwt
{
    using System;

    public interface IJwtUserData
    {
        Guid Id { get; }

        DateTime CreatedOn { get; }
        Guid? CreatedBy { get; }

        string Email { get; }

        Roles Role { get; }
        bool IsActive { get; }
    }
}
