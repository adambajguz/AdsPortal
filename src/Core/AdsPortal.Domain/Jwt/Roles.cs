namespace AdsPortal.Domain.Jwt
{
    using System;

    [Flags]
    public enum Roles : uint
    {
        None = 0,
        ResetPassword = 1,
        User = 2,
        Editor = 4,
        Admin = 8
    }
}
