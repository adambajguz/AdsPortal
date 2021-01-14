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

        [RenderableProperty(DisplayName = "Created on")]
        public DateTime CreatedOn { get; set; }

        [RenderableProperty(DisplayName = "Created by")]
        public Guid? CreatedBy { get; set; }

        [RenderableProperty(DisplayName = "Last saved on")]
        public DateTime LastSavedOn { get; set; }

        [RenderableProperty(DisplayName = "Last saved by")]
        public Guid? LastSavedBy { get; set; }

        [RenderableProperty(DisplayName = "e-mail")]
        public string Email { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;

        public Roles Role { get; set; }
    }
}
