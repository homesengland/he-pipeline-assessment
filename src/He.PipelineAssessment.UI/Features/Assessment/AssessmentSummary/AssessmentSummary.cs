namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryData
    {
        public string Id { get; set; } = null!;
        public string SiteName { get; set; } = null!;
        public string Project { get; set; } = null!;
        public string Partner { get; set; } = null!;
        public string ProjectManager { get; set; } = null!;
        public IEnumerable<AssessmentStage> Stages { get; set; } = null!;
    }

    public class AssessmentStage
    {
        public string StageId { get; set; } = null!;
        public string StageName { get; set; } = null!;
         
        public AssessmentSummaryStatus Status { get; set; }

        public DateTime? StartedOn { get; set; }

        public string SubmittedBy { get; set; } = null!;

        public DateTime? Submitted { get; set; }

        public string Result { get; set;  } = null!;

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
    }

    public enum AssessmentSummaryStatus
    {
        Draft,
        Submitted
    }
}
