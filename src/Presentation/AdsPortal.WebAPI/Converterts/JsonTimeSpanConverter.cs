namespace AdsPortal.WebAPI.Converterts
{
    using System;
    using System.Diagnostics;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        [DebuggerHidden]
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                string str = reader.GetString();

                return TimeSpan.Parse(str);
            }
            catch (Exception ex)
            {
                throw new JsonException("TimeSpan conversion failed", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            string str = value.ToString();
            writer.WriteStringValue(str);
        }
    }
}
