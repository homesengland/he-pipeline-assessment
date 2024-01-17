namespace Elsa.CustomModels
{
    public class QuestionDataDictionaryGroup : AuditableEntity
    {
        //public int Id { get; set; }
        public int TempId { get; set; }
        public int PlaceholderId { get; set; }
        public string Name { get; set; } = null!;
        //public virtual List<QuestionDataDictionary> QuestionDataDictionaryList { get; set; } = new List<QuestionDataDictionary>();
    }
}
