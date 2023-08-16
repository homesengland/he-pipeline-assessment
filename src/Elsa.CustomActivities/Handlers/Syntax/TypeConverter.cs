using Elsa.CustomActivities.Activities.QuestionScreen;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Native;
using Jint.Runtime;

namespace Elsa.CustomActivities.Handlers.Syntax
{
    public class TypeConverter : JsonConverter<Type>
    {
        public override Type? ReadJson(JsonReader reader, Type objectType, Type? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? typeName = (string)reader.Value!;
            if (typeName == null)
            {
                return null;
            }
            Type? type = Type.GetType(typeName);
            return type ?? null;
        }

        public override void WriteJson(JsonWriter writer, Type? value, JsonSerializer serializer)
        {
            if(value != null)
            {
                writer.WriteValue(value.Name);
            }
            writer.WriteNull();

        }
    }
}
