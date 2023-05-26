using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class CreateAssessmentInterventionDto
    {
        public AssessmentInterventionCommand CreateAssessmentInterventionCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
        public List<TargetWorkflowDefinition> TargetWorkflowDefinitions { get; set; }
    }



    public class TargetWorkflowDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string DisplayName => $"{Name} ({Id})";
    }
}
