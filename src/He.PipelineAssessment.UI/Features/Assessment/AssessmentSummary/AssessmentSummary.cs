using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist;

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
        public string? BusinessArea { get; set; }
        public string? ProjectManager { get; set; }
        public string? SensitiveStatus { get; set; }
        public bool HasValidBusinessArea { get; set; }
        public List<string>? BusinessAreaMessage { get; set; }
        public IEnumerable<AssessmentSummaryStage> Stages { get; set; } = null!;
        public IEnumerable<AssessmentInterventionViewModel> Interventions { get; set; } = null!;
        public IEnumerable<AssessmentSummaryStage> StagesHistory { get; set; } = null!;
        public IEnumerable<SensitiveRecordPermissionsWhitelistDto> Permissions { get; set; } = Enumerable.Empty<SensitiveRecordPermissionsWhitelistDto>();

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
        public string? FirstActivityId { get; set; } = null!;
        public string? FirstActivityType { get; set; } = null!;
        public string? Status { get; set; } = null!;
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public int? AssessmentToolId { get; set; }
        public int? AssessmentToolWorkflowId { get; set; }
        public int? AssessmentToolWorkflowInstanceId { get; set; }
        public bool? IsFirstWorkflow { get; set; }
        public bool? IsVariation { get; set; }
        public bool? IsEarlyStage { get; set; }
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
                case AssessmentToolWorkflowInstanceConstants.Draft:
                    return "blue";
                case AssessmentToolWorkflowInstanceConstants.Submitted:
                    return "green";
                case AssessmentToolWorkflowInstanceConstants.SuspendedRollBack:
                    return "red";
                case AssessmentToolWorkflowInstanceConstants.SuspendedAmendment:
                    return "red";
                case AssessmentToolWorkflowInstanceConstants.SuspendOverrides:
                    return "red";
            }
            return "grey";
        }
    }
}
