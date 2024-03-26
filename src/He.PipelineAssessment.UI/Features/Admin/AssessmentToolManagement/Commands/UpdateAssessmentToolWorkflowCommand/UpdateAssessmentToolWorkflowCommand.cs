using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand
{
    public class UpdateAssessmentToolWorkflowDto
    {
        public UpdateAssessmentToolWorkflowCommand UpdateAssessmentToolWorkflowCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
    }

    public class UpdateAssessmentToolWorkflowCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        public bool IsFirstWorkflow { get; set; }
        public bool IsEconomistWorkflow { get; set; }
        public int AssessmentToolId { get; set; }
        public int Version { get; set; } = 1;
        public bool IsLatest { get; set; } = true;
        public bool IsVariation { get; set; }
        public bool IsAmendableWorkflow { get; set; } = true;
        public bool IsLast { get; set; }
        public bool IsEarlyStage {  get; set; }
    }
}
