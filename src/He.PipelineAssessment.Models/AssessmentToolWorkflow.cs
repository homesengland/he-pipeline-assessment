﻿namespace He.PipelineAssessment.Models
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
        public string? Status { get; set; }

        public virtual AssessmentTool AssessmentTool { get; set; } = null!;

        public virtual List<AssessmentIntervention>? AssessmentInterventions { get; set; }
        
    }
}
