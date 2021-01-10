namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces
{
    public interface IArgumentsSerializer
    {
        string? Serialize(object? obj);

        object? Deserialize(string? json);
    }
}