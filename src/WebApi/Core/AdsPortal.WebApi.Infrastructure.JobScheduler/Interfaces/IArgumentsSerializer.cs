namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces
{
    using System;

    public interface IArgumentsSerializer
    {
        string Serialize(object? obj);

        object? Deserialize(string? json);
    }
}