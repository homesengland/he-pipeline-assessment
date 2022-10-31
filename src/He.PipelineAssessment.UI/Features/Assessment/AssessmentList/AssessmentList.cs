namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentList
{
    public class AssessmentListData
    {
        public List<Assessment> ListOfAssessments { get; set; } = null!;
    }

    public class Assessment
    {
        public string Id { get; set; } = null!;
        public string ProjectName { get; set; } = null!;

        public string ProjectManager { get; set; } = null!;

        public string Partner { get; set; } = null!;

        public string Team { get; set; } = null!;

        public string LocalAuthority { get; set; } = null!;

        public DateTime DateCreated { get; set; }

        public AssessmentStatus Status { get; set; }

        public string AssessmentWorkflowId { get; set; } = null!;

        public string StatusDisplay()
        {
            switch (Status)
            {
                case AssessmentStatus.New:
                    return "New";
                case AssessmentStatus.In_Progress:
                    return "In Progress";
                case AssessmentStatus.Complete:
                    return "Complete";
                case AssessmentStatus.Stopped:
                    return "Stopped";
            }
            return "Unknown";
        }

        public string StatusDisplayTag()
        {
            switch (Status)
            {
                case AssessmentStatus.New:
                    return "blue";
                case AssessmentStatus.In_Progress:
                    return "yellow";
                case AssessmentStatus.Complete:
                    return "green";
                case AssessmentStatus.Stopped:
                    return "red";
            }
            return "grey";
        }
    }

    public enum AssessmentStatus
    {
        New,
        In_Progress,
        Stopped,
        Complete
    }
}
