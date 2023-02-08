using He.PipelineAssessment.Infrastructure.Migrations;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListData
    {
        public List<string> ValidationMessages { get; set; } = new List<string>();
        public bool IsValid { get { return ValidationMessages != null && ValidationMessages.Count == 0; } }
        public List<AssessmentDisplay> ListOfAssessments { get; set; } = new List<AssessmentDisplay>();
    }

    public class AssessmentDisplay
    {

        public AssessmentDisplay()
        {
            Status = "New";
        }
        public AssessmentDisplay(Models.Assessment assessment)
        {
            Id = assessment.Id.ToString();
            SpId = assessment.SpId.ToString();
            SiteName = assessment.SiteName;
            ProjectManager = assessment.ProjectManager;
            Counterparty = assessment.Counterparty;
            Status = assessment.Status;
            Reference = assessment.Reference;
            LocalAuthority =  assessment.LocalAuthority;
            FundingAsk = string.Format(new CultureInfo("en-GB", false), "{0:c0}", assessment.FundingAsk);           
            NumberOfHomes = assessment.NumberOfHomes;
        }
        

        public string Id { get; set; } = null!;
        public string SpId { get; set; } = null!;
        public string SiteName { get; set; } = null!;

        public string ProjectManager { get; set; } = null!;

        public string Counterparty { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public string Reference { get; set; } = null!;
        public string Status { get; set; }

        public string LocalAuthority { get; set; } = null!;
       
        public string? FundingAsk { get; set; }
        public int? NumberOfHomes { get; set; }
        public DateTime? LastModified { get; set; } 
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
