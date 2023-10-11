using FluentValidation.Results;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public bool IsValid { get; set; }
        public ValidationResult? ValidationMessages { get; set; }
    }
}
