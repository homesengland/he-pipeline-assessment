using NodaTime;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elsa.Server.Helpers
{
    public class DateTimeConverterUsingDateTimeParse : JsonConverter<Instant>
    {
        //public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    Debug.Assert(typeToConvert == typeof(DateTime));
        //    return DateTime.Parse(reader.GetString() ?? string.Empty);
        //}

        public override Instant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                Debug.Assert(typeToConvert == typeof(Instant));
                string? value = reader.GetString();
                if (value != null)
                {
                    var offset = DateTime.Parse(value);
                    return Instant.FromDateTimeOffset(offset);
                }
                var dateTime = DateTime.Parse(string.Empty);
                return Instant.FromDateTimeOffset(dateTime);
            }
            catch(InvalidOperationException ex)
            {
                return Instant.FromDateTimeUtc(DateTime.Now);
            }


        }

        public override void Write(Utf8JsonWriter writer, Instant value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToDateTimeOffset().ToString());
        }
    }
}

