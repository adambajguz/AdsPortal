namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [DetailsOperation(ResponseType = typeof(UserDetails), DisplayName = "User details")]
    public class GetUserDetails
    {
        public Guid Id { get; set; }
    }

    [RenderableClass]
    public class UserDetails
    {
        [RenderableProperty(DisplayName = "Actions", Renderer = typeof(DetailsControlsRenderer))]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Email { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Description { get; set; } = default!;

        public Roles Role { get; set; }
    }
}
