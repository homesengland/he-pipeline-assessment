using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentList
{
    public class AssessmentListData
    {
        public List<AssessmentDisplay> ListOfAssessments { get; set; } = new List<AssessmentDisplay>();
    }

    public class AssessmentDisplay
    {

        public AssessmentDisplay()
        {

        }
        public AssessmentDisplay(Assessment assessment)
        {
            Id = assessment.Id.ToString();
            SpId = assessment.SpId.ToString();
            SiteName = assessment.SiteName;
            ProjectManager = assessment.ProjectManager;
            Counterparty = assessment.Counterparty;
            Status = assessment.Status;
            AssessmentWorkflowId = assessment.WorkflowInstanceId;
            
        }

        public string Id { get; set; } = null!;
        public string SpId { get; set; } = null!;
        public string SiteName { get; set; } = null!;

        public string ProjectManager { get; set; } = null!;

        public string Counterparty { get; set; } = null!;

        public DateTime DateCreated { get; set; }

        public string Status { get; set; }

        public string? AssessmentWorkflowId { get; set; } = null!;

        public string StatusDisplayTag()
        {
            switch (Status)
            {
                case "New":
                    return "blue";
                case "In Progress":
                    return "yellow";
                case "Complete":
                    return "green";
            }
            return "grey";
        }
    }
}
