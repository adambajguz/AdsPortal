namespace AdsPortal.WebPortal.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseApiController : ControllerBase
    {
        //private readonly Lazy<IMediator> _mediator;

        //protected IMediator Mediator => _mediator.Value;

        //protected BaseApiController()
        //{
        //    _mediator = new Lazy<IMediator>(() => HttpContext.RequestServices.GetRequiredService<IMediator>());
        //}
    }
}
