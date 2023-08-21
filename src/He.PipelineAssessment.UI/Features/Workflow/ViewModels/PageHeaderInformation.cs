﻿using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class PageHeaderInformation : WorkflowActivityDataDto
    {
        public int AssessmentId { get; set; }
        public int CorrelationId { get; set; }
        public string SiteName { get; set; } = null!;
        public string CounterParty { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string? LocalAuthority { get; set; }
        public string? ProjectManager { get; set; }
    }
}
