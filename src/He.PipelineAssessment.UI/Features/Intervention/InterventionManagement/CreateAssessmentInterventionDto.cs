using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class CreateAssessmentInterventionDto
    {
        public CreateAssessmentInterventionCommand CreateAssessmentInterventionCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
        public List<TargetWorkflowDefinition> TargetWorkflowDefinitions { get; set; }
    }

    public class CreateAssessmentInterventionCommand : IRequest<int>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string AssessmentName { get; set; } = null!;
        public string? AssessmentResult { get; set; }
        public string? ProjectReference { get; set; }
        public string? RequestedBy { get; set; }
        public string? RequestedByEmail { get; set; }
        public string? Administrator { get; set; }
        public string? AdministratorEmail { get; set; }
        public string? SignOffDocument { get; set; }
        public string DecisionType { get; set; } = null!;
        public string? AssessorRationale { get; set; }
        public string? AdministratorRationale { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string? Status { get; set; }
        public string? TargetWorkflowDefinitionId { get; set; }
    }

    public class TargetWorkflowDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string DisplayName => $"{Name} ({Id})";
    }
}
