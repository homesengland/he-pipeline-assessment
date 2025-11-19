using MediatR;
using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentFund
{
    public class CreateAssessmentFundCommandDto
    {
        public CreateAssessmentFundCommand CreateAssessmentFundCommand { get; set; } = new();

        //COMMENT: Insert validation here.
        //public ValidationResult? ValidationResult { get; set; }
    }
    public class CreateAssessmentFundCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public bool IsEarlyStage { get; set; } = false;
        public bool IsDisabled { get; set; } = false;
    }
}
