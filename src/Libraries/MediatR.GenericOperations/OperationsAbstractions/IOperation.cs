namespace AdsPortal.Application.OperationsAbstractions
{
    using MediatR;

    public interface IOperation : IRequest
    {

    }

    public interface IOperation<out TResult> : IRequest<TResult>
        where TResult : class, IOperationResult
    {

    }
}
