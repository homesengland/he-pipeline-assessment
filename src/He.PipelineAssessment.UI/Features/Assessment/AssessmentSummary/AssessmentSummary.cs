namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryResponse
    {
        public int AssessmentId { get; set; }
        public int CorrelationId { get; set; }
        public string SiteName { get; set; } = null!;
        public string CounterParty { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public IEnumerable<AssessmentSummaryStage> Stages { get; set; } = null!;
    }

    public class AssessmentSummaryStage
    {
        public int StageId { get; set; }
        public string StageName { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime? StartedOn { get; set; }

        public string SubmittedBy { get; set; } = null!;

        public DateTime? Submitted { get; set; }

        public string WorkflowInstanceId { get; set; } = null!;
        public string CurrentActivityId { get; set; } = null!;
        public string CurrentActivityType { get; set; } = null!;


        public string StartedDateString()
        {
            return AssessmentDateToString(StartedOn);
        }

        public string SubmittedDateString()
        {
            return AssessmentDateToString(Submitted);
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
