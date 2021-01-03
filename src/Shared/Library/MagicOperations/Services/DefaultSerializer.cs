namespace MagicOperations.Services
{
    using System;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using Newtonsoft.Json;

    public class DefaultSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public DefaultSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };
        }

        public string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, _settings);
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializerException($"Faield to serialize object to JSON: {ex.Message}", ex);
            }
        }

        public T? Deserialize<T>(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializerException($"Faield to deserialize JSON: {ex.Message}", ex);
            }
        }

        public object? Deserialize(Type type, string json)
        {
            try
            {
                return JsonConvert.DeserializeObject(json, type, _settings);
            }
            catch (JsonSerializationException ex)
            {
                throw new SerializerException($"Faield to deserialize JSON: {ex.Message}", ex);
            }
        }
    }
}
