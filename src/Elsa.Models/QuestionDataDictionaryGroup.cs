namespace Elsa.CustomModels
{
    public class QuestionDataDictionaryGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<QuestionDataDictionary>? QuestionDataDictionaryList { get; set; }
    }
}
