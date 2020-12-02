namespace AdsPortal.Application.Interfaces.Persistence.UoW
{
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Application.Interfaces.Persistence.UoW.Generic;

    public interface IMongoUnitOfWork : IGenericMongoUnitOfWork
    {
        IAnalyticsRecordsRepository AnalyticsRecords { get; }
    }
}
