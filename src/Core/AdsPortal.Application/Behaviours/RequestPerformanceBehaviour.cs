namespace AdsPortal.Application.Behaviours
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        //private readonly ICurrentUserService _currentUserService;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)//, ICurrentUserService currentUserService)
        {
            _timer = new Stopwatch();

            _logger = logger;
            //_currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            TResponse response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 500)
            {
                string name = typeof(TRequest).Name;

                _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    name, _timer.ElapsedMilliseconds, request);
            }

            return response;
        }
    }
}
