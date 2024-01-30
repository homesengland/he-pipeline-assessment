namespace Elsa.CustomModels
{
    public class DataDictionaryGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsArchived { get; set; } = false;
        public virtual List<DataDictionary> DataDictionaryList { get; set; } = new List<DataDictionary>();
    }
}
