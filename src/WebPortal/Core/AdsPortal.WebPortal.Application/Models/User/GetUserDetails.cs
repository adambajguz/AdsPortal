namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [DetailsOperation(ResponseType = typeof(UserDetails))]
    public class GetUserDetails
    {

    }

    [RenderableClass]
    public class UserDetails
    {
        public Guid Id { get; init; }
        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Email { get; init; } = default!;

        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
        public string Description { get; init; } = default!;

        public Roles Role { get; init; }
    }
}
