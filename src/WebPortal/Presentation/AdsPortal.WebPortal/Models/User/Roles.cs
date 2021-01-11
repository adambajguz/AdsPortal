namespace AdsPortal.WebPortal.Models.User
{
    using System;

    [Flags]
    public enum Roles : uint
    {
        None = 0,
        ResetPassword = 1,
        User = 2,
        Admin = 4
    }
}
