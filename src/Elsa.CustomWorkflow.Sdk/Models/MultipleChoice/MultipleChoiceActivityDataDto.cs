using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace Elsa.CustomWorkflow.Sdk.Models.MultipleChoice
{
    public  class MultipleChoiceActivityDataDto : WorkflowActivityDataDto<MultipleChoiceQuestionDataDto>
    {
        public MultipleChoiceQuestionDataDto ActivityData { get; set; } = null!;
    }

    public class MultipleChoiceQuestionDataDto : QuestionActivityDataDto
    {
        public Choice[] Choices { get; set; } = null!;
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
