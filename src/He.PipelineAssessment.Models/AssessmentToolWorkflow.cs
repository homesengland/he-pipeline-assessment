using System.Text.Json.Serialization;

namespace He.PipelineAssessment.Models
{
    public class AssessmentToolWorkflow : AuditableEntity
    {
        public int Id { get; set; }
        public int AssessmentToolId { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;
        public bool IsFirstWorkflow { get; set; }
        public int Version { get; set; }
        public bool IsLatest { get; set; }
        public string Name { get; set; } = null!;
        public bool IsEconomistWorkflow { get; set; }
        public bool IsVariation { get; set; }
        public bool IsEarlyStage { get; set; }
        public bool IsLast { get; set; }
        public string? Status { get; set; }
        public bool IsAmendable { get; set; }
        public int? AssessmentFundId { get; set; }

        public virtual AssessmentTool AssessmentTool { get; set; } = null!;
        [JsonIgnore]
        public virtual List<TargetAssessmentToolWorkflow>? TargetAssessmentToolWorkflows { get; set; }

        public virtual AssessmentFund? AssessmentFund { get; set; }

        //Comment: need to add fundID
    }
}
