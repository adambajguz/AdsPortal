namespace AdsPortal.Application.GenericHandlers.Relational.Commands
{
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Application.OperationsModels.Core;
    using AdsPortal.Domain.Mapping;

    public interface ICreateCommand : IOperation<IdResult>, ICustomMapping
    {

    }
}
