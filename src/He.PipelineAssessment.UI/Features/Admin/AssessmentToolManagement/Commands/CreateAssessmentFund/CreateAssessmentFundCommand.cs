using MediatR;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentFund
{
    public class CreateAssessmentFundDto
    {
        public CreateAssessmentFundCommand CreateAssessmentFundCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
    }
    public class CreateAssessmentFundCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public bool IsEarlyStage { get; set; } = false;
        public string Description { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } = false;
    }
}
