using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers.Models;

namespace Elsa.CustomActivities.Handlers.ParseModels
{
        public record ActivityListModelRaw
        {
            public List<ActivityElementRaw> Activities { get; set; } = null!;
        }
        public record ActivityElementRaw
        {
            public ElsaProperty Value { get; set; } = null!;
            public List<HeActivityInputDescriptor> Descriptor { get; set; } = null!;
            public ActivityTypeModel ActivityType { get; set; } = null!;
        }

        public record ActivityTypeModel(string NameConstant, string DisplayName, string Description);

        public record ActivityPropertyRaw(ElsaProperty Value, HeActivityInputDescriptor Descriptor);


        public record ActivityElementProperties
        {
            public List<ElsaProperty> ActivityProperties { get; set; } = null!;
            public string ActivityType { get; set; } = null!;
        }

        public record ActivityElement
        {
            public ElsaProperty Activity { get; set; } = null!;
            public string ActivityType { get; set; } = null!;
        }

    public record ActivityChoiceModel
    {
        public List<ElsaProperty> Json { get; set; } = null!;
    }

}
