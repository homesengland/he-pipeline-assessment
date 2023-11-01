using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class AssessmentInterventionCommand
    {
        public int AssessmentId { get; set; }
        public int CorrelationId { get; set; }
        public int AssessmentInterventionId { get; set; }
        public int AssessmentToolWorkflowInstanceId { get; set; }
        public string WorkflowInstanceId { get; set; } = null!;
        public string AssessmentName { get; set; } = null!;
        public string? AssessmentResult { get; set; }
        public string? ProjectReference { get; set; }
        public string RequestedBy { get; set; } = null!;
        public string RequestedByEmail { get; set; } = null!;
        public string? Administrator { get; set; }
        public string? AdministratorEmail { get; set; }
        public string? SignOffDocument { get; set; }
        public string DecisionType { get; set; } = null!;
        public string? AssessorRationale { get; set; }
        public string? AdministratorRationale { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; } = null!;
        public int? TargetWorkflowId { get; set; }
        public List<TargetWorkflowDefinition> TargetWorkflowDefinitions { get; set; } = new();

        public int? InterventionReasonId { get; set; }
        public string? InterventionReasonName { get; set; }
        public virtual string FinalInstanceStatus => AssessmentToolWorkflowInstanceConstants.Submitted;
        public List<TargetWorkflowDefinition> SelectedWorkflowDefinitions { get; set; } = new();
    }
}
