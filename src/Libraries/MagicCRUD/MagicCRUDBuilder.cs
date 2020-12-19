namespace MagicCRUD
{
    using AdsPortal.Domain.Abstractions.Base;

    public sealed class MagicCRUDBuilder
    {
        public MagicCRUDBuilder()
        {

        }

        public MagicCRUDBuilder CreateOperationsGroup<TEntity>()
            where TEntity : IBaseIdentifiableEntity
        {


            return this;
        }
    }
}
