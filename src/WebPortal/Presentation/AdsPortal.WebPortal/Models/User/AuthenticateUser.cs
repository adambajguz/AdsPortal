namespace AdsPortal.WebPortal.Models.User
{
    using MagicModels.Attributes;

    [RenderableClass]
    public class AuthenticateUser
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
