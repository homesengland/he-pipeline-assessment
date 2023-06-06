using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class AssessmentInterventionDto
    {
        public AssessmentInterventionCommand AssessmentInterventionCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
        public List<TargetWorkflowDefinition>? TargetWorkflowDefinitions { get; set; }
    }



    public class TargetWorkflowDefinition
    {
        public int Id { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string DisplayName => $"{Name} ({WorkflowDefinitionId})";
    }

    public struct DtoConfig
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string? AdministratorName { get; set; }
        public string? AdministratorEmail { get; set; }
        public string DecisionType { get; set; }
        public string Status { get; set; }
    }
}
