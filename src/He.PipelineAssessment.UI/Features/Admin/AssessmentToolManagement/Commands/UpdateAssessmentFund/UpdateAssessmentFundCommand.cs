using MediatR;
using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentFund
{
    public class UpdateAssessmentFundCommandDTO
    {
        //COMMENT: UpdateAssessmentFund is a new instance of UpdateAssessmentFundCommand class.
        public UpdateAssessmentFundCommand UpdateAssessmentFundCommand { get; set; } = new();

        //COMMENT: ValidationResult is a property of type ValidationResult? that can either hold validation results or be null.
       // public ValidationResult? ValidationResult { get; set; }
    }

    //COMMENT: UpdateAssessmentFundCommand is a class that implements the IRequest interface from MediatR, specifying that it will return an integer when handled.
    // COMMENT: IRequest returns the ID of the updated assessment fund.
    public class UpdateAssessmentFundCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsEarlyStage { get; set; }
        public bool IsDisabled { get; set; }
    }
}
