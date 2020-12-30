namespace MediatR.GenericOperations.Commands
{
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public interface IUpdateCommand : IIdentifiableOperation, ICustomMapping
    {

    }
}
