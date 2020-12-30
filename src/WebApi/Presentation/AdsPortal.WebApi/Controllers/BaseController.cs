namespace AdsPortal.WebAPI.Controllers
{
    using System;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
        private readonly Lazy<IMediator> _mediator;

        protected IMediator Mediator => _mediator.Value;

        protected BaseController()
        {
            _mediator = new Lazy<IMediator>(() => HttpContext.RequestServices.GetRequiredService<IMediator>());
        }
    }
}
