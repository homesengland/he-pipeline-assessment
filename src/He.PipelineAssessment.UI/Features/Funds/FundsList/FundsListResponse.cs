using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Funds.FundsList
{
    public class FundsListResponse
    {
        // list of funds. 
        public List<AssessmentFund> Funds { get; set; } = null!;

        // constructor
        public FundsListResponse(List<AssessmentFund> funds)
        {
            Funds = funds;
        }

    }
}
