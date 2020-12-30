namespace AdsPortal.WebPortal.Application.Models.User
{
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [CreateOperation]
    public class CreateUser
    {
        public string? Email { get; init; }
        public string? Password { get; init; }

        public string? Name { get; init; }
        public string? Surname { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Address { get; init; }

        [OperationPropertyIgnore]
        public Roles Role { get; init; }
    }
}
