using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace Elsa.CustomWorkflow.Sdk.Models.Text
{
    public  class TextActivityDataDto : WorkflowActivityDataDto<TextQuestionDataDto>
    {
        public TextQuestionDataDto ActivityData { get; set; } = null!;
    }

    public class TextQuestionDataDto : QuestionActivityDataDto
    {
        public string Answer { get; set; } = null!;
    }
}
