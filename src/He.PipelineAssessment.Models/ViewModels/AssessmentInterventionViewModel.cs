using He.PipelineAssessment.Models.Helper;

namespace He.PipelineAssessment.Models.ViewModels;

public class AssessmentInterventionViewModel : AuditableEntityViewModel
{
    public int Id { get; set; }
    public string Reference { get; set; } = null!;
    public string WorkflowName { get; set; } = null!;
    public string RequestedBy { get; set; } = null!; 
    public string? Administrator { get; set; }
    public string Status { get; set; } = null!;
    public string DecisionType { get; set; } = null!;
    public string? SensitiveStatus { get; set; } = null!;
    public string ProjectManager { get; set; } = null!;
    public bool ValidData { get; set; }

    public string StatusDisplayTag()
    {
        switch (Status)
        {
            case InterventionStatus.Pending:
                return "blue";
            case InterventionStatus.Approved:
                return "green";
            case InterventionStatus.Rejected:
                return "red";
        }
        return "yellow";
    }

    public bool IsSensitiveRecord()
    {
        return SensitiveStatusHelper.IsSensitiveStatus(SensitiveStatus);
    }
}