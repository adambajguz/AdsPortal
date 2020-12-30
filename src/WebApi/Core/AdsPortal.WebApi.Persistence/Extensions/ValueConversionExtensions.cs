namespace AdsPortal.Persistence.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;

    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
            where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<T>(x) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
                x => x == null ? 0 : JsonConvert.SerializeObject(x).GetHashCode(),
                x => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(x))
            );

            propertyBuilder.HasConversion(converter)
                           .HasColumnType("nvarchar(max)");

            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}
