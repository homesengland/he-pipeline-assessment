using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class QuestionScreenSaveAndContinueCommandResponse : IRequest<QuestionScreenSaveAndContinueCommand>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public bool IsAuthorised { get; set; }
        public bool IsValid { get; set; }
        public ValidationResult? ValidationMessages { get; set; }
    }
}
