namespace He.PipelineAssessment.Models.ViewModels;

public class AssessmentInterventionViewModel : AuditableEntityViewModel
{
    public int Id { get; set; }
    public string Reference { get; set; }
    public string WorkflowName { get; set; }
    public string RequestedBy { get; set; }
    public string Administrator { get; set; }
    public string Status { get; set; }
    public string DecisionType { get; set; }

    public string StatusDisplayTag()
    {
        switch (Status)
        {
            case InterventionStatus.NotSubmitted:
            case InterventionStatus.Pending:
                return "blue";
            case InterventionStatus.Approved:
                return "green";
            case InterventionStatus.Rejected:
                return "red";
        }
        return "yellow";
    }
}