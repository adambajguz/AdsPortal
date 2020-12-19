namespace MagicCRUD
{
    using AdsPortal.Application.GenericHandlers.Relational.Commands;

    public sealed class OperationGroupBuilder
    {
        public OperationGroupBuilder()
        {

        }

        public OperationGroupBuilder AddCrateOperation<TDto>()
            where TDto : ICreateCommand
        {


            return this;
        }
    }
}
