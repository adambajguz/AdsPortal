namespace AdsPortal.WebApi.Infrastructure.Mailing.Services.Renderer
{
    using System.Dynamic;

    /// <summary>
    /// We have to remove reference to FluentEmail.Razor (not support core 3.0)
    /// For now this is only one thing we need from this lib
    /// </summary>
    public interface IViewBagModel
    {
        ExpandoObject ViewBag { get; }
    }
}
