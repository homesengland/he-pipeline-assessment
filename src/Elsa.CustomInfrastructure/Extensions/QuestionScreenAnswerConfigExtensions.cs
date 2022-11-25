using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elsa.CustomInfrastructure.Extensions
{
    public static class QuestionScreenAnswerConfigExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<T>(v, options) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonSerializer.Serialize(l, options) == JsonSerializer.Serialize(r, options),
                v => v == null ? 0 : JsonSerializer.Serialize(v, options).GetHashCode(),
                v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, options), options)
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("nvarchar(max)");

            return propertyBuilder;
        }
    }
}
