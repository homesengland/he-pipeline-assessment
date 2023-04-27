namespace Elsa.CustomModels
{
    public class QuestionChoiceGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string GroupIdentifier { get; set; } = null!;
        public List<QuestionChoice> QuestionGroupChoices { get; set; }
    }
}
