namespace MediatR.GenericOperations.Commands
{
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;
    using MediatR.GenericOperations.Models;

    public interface ICreateCommand : IOperation<IdResult>, ICustomMapping
    {

    }
}
