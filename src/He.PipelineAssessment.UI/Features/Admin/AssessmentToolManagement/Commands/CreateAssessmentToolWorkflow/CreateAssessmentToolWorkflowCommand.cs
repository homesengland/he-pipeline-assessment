using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow
{
    public class CreateAssessmentToolWorkflowDto
    {
        public int AssessmentToolId { get; set; }

        public CreateAssessmentToolWorkflowCommand CreateAssessmentToolWorkflowCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }

    }
    public class CreateAssessmentToolWorkflowCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        public bool IsFirstWorkflow { get; set; } = false;
        public int AssessmentToolId { get; set; }
        public int Version { get; set; } = 1;
        public bool IsLatest { get; set; } = true;
    }
}
