using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Funds.ViewModels;

namespace He.PipelineAssessment.UI.Features.Funds.FundsList
{
    public class FundsListResponse
    {
        // list of funds. 
        public List<AssessmentFundsDTO> Funds { get; set; } = null!;

        // constructor
        public FundsListResponse(List<AssessmentFund> dbFunds)
        {
            Funds = new List<AssessmentFundsDTO>();

            foreach (var dbFund in dbFunds)
            {
                Funds.Add(new AssessmentFundsDTO
                {
                    Id = dbFund.Id,
                    Name = dbFund.Name,
                    IsEarlyStage = dbFund.IsEarlyStage,
                    IsDisabled = dbFund.IsDisabled
                });
            }

            
        }

    }
}
