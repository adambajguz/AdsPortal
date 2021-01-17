namespace AdsPortal.WebApi.Domain.Enums
{
    public enum JobStatuses
    {
        Queued,
        Taken,
        Running,
        Cancelled,
        TimedOut,
        Success,
        Error,
        MaxRetriesReached
    }
}
