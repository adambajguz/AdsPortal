namespace MediatR.GenericOperations.Abstractions
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
