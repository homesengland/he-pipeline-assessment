using System.Linq;

namespace Elsa.Server.Extensions
{
    public static class ActivityDataDictionaryExtensions
    {
        public static object? GetData(this IDictionary<string, object?> dataDictionary, string property)
        {
            return dataDictionary.FirstOrDefault(x => x.Key == property).Value;
        }

    }
}
