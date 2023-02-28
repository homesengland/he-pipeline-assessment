using He.PipelineAssessment.Models.ViewModels;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryResponse
    {
        public int AssessmentId { get; set; }
        public int CorrelationId { get; set; }
        public string SiteName { get; set; } = null!;
        public string CounterParty { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string? LocalAuthority { get; set; }
        public string? ProjectManager { get; set; }   
        public IEnumerable<AssessmentSummaryStage> Stages { get; set; } = null!;

    }

    public class AssessmentSummaryStage
    {
        public string Name { get; set; } = null!;
        public bool IsVisible { get; set; }
        public int Order { get; set; }
        public string? WorkflowName { get; set; } = null!;
        public string? WorkflowDefinitionId { get; set; } = null!;
        public string? WorkflowInstanceId { get; set; } = null!;
        public string? CurrentActivityId { get; set; } = null!;
        public string? CurrentActivityType { get; set; } = null!;
        public string? Status { get; set; } = null!;
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public int? AssessmentToolId { get; set; }
        public int? AssessmentToolWorkflowInstanceId { get; set; }
        public bool? IsFirstWorkflow { get; set; }
        public string? Result { get; set; } = null;
        public string? SubmittedBy { get; set; } = null;

        public string StartedDateString()
        {
            return AssessmentDateToString(CreatedDateTime);
        }

        public string SubmittedDateString()
        {
            return AssessmentDateToString(SubmittedDateTime);
        }

        private string AssessmentDateToString(DateTime? dateToFormat)
        {
            if (dateToFormat != null && dateToFormat != default)
            {
                return dateToFormat.Value.ToString("d/MM/yy");
            }
            else return "";
        }

        public string StatusDisplayTag()
        {
            switch (Status)
            {
                case "Draft":
                    return "blue";
                case "Submitted":
                    return "green";
            }
            return "grey";
        }
    }
}
