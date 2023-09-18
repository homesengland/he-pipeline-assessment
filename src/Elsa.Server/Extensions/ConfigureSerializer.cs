using Newtonsoft.Json;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace Elsa.Server.Extensions
{
    public static class ConfigureSerializer
    {
        public static JsonSerializerSettings ConfigureForInstants(this JsonSerializerSettings @this)
        {
            @this.DateParseHandling = DateParseHandling.None;
            @this.NullValueHandling = NullValueHandling.Ignore;
            @this.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            return @this;
        }

    }
}
