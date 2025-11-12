using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentFund
{
    public record DeleteAssessmentFundCommand(int Id) : IRequest<int>;
    
}
