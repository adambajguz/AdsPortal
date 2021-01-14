namespace AdsPortal.WebPortal.Models.User
{
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using MagicModels.Attributes;
    using MagicModels.Components.Defaults.PropertyRenderers;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [CreateOperation(ResponseType = typeof(IdResult), DisplayName = "Create user")]
    public class CreateUser
    {
        public string? Email { get; set; }

        [RenderableProperty(Renderer = typeof(PasswordRenderer))]
        public string? Password { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }

        public Roles Role { get; } = Roles.User;
    }
}
