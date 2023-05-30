using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class AssessmentInterventionCommand
    {
        public int AssessmentInterventionId { get; set; }
        public int AssessmentToolWorkflowInstanceId { get; set; }
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
        public int TargetWorkflowId { get; set; }
        public string? TargetWorkflowDefinitionId { get; set; }
        public string? TargetWorkflowDefinitionName { get; set; }
    }
}
