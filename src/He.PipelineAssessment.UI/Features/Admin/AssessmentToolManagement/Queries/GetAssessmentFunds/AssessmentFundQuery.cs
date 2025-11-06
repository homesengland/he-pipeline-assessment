using He.PipelineAssessment.UI.Features.Funds.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentFunds
{
    public record AssessmentFundQuery (int AssessmentFundId) : IRequest<AssessmentFundsDTO>
    {
    }
}

