namespace AdsPortal.Domain.Jwt
{
    using System;

    public interface IJwtUserData
    {
        Guid Id { get; set; }

        DateTime CreatedOn { get; set; }
        Guid? CreatedBy { get; set; }

        string Email { get; set; }

        Roles Role { get; set; }
        bool IsActive { get; set; }
    }
}
