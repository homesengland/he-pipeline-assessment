using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentFund
{
    // COMMENT: DeleteAssessmentFundCommand is a record type that implements the IRequest interface from MediatR, specifying that it will return an integer when handled.
    public record DeleteAssessmentFundCommand(int Id) : IRequest<int>;
    
}
