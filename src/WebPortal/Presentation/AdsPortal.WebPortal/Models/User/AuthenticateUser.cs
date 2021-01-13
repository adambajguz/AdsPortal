namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [LoginOperation(ResponseType = typeof(AuthenticateUserResponse), DisplayName = "Login user")]
    public class AuthenticateUser
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    [RenderableClass]
    public class AuthenticateUserResponse
    {
        public string Token { get; init; } = default!;
        public TimeSpan Lease { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
