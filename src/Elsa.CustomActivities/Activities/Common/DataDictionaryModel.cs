namespace Elsa.CustomActivities.Activities.Common
{
    
    public class DataDictionaryModel
    {
        public DataDictionaryRecord? DataDictionary { get; set; }
    }
    public record DataDictionaryRecord(string Identifier, string Name);
}
