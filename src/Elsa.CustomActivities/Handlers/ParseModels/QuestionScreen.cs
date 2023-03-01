using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers.Models;

namespace Elsa.CustomActivities.Handlers.ParseModels
{
        public record QuestionListModelRaw
        {
            public List<QuestionElementRaw> Questions { get; set; } = null!;
        }
        public record QuestionElementRaw
        {
            public ElsaProperty Value { get; set; } = null!;
            public List<HeActivityInputDescriptor> Descriptor { get; set; } = null!;
            public QuestionTypeModel QuestionType { get; set; } = null!;
        }

        public record QuestionTypeModel(string NameConstant, string DisplayName, string Description);

        public record QuestionPropertyRaw(ElsaProperty Value, HeActivityInputDescriptor Descriptor);


        public record QuestionElementProperties
        {
            public List<ElsaProperty> QuestionProperties { get; set; } = null!;
            public string QuestionType { get; set; } = null!;
        }

        public record QuestionElement
        {
            public ElsaProperty Question { get; set; } = null!;
            public string QuestionType { get; set; } = null!;
        }

    public record ChoiceModel
    {
        public List<ElsaProperty> Json { get; set; } = null!;
    }

}
