namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Exceptions;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces;
    using Newtonsoft.Json;

    public class DefaultArgumentsSerializer : IArgumentsSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public DefaultArgumentsSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                MissingMemberHandling = MissingMemberHandling.Error,
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };
        }

        public string Serialize(object? obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, _settings);
            }
            catch (JsonSerializationException ex)
            {
                throw new ArgumentsSerializerException($"Failed to serialize object to JSON: {ex.Message}", ex);
            }
        }

        public object? Deserialize(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<object?>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                throw new ArgumentsSerializerException($"Failed to deserialize JSON: {ex.Message}", ex);
            }
        }
    }
}
