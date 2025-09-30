using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using MediatR;
using System.Runtime.CompilerServices;

namespace He.PipelineAssessment.UI.Features.Funds.FundsList
{
    public class FundsListRequestHandler : IRequestHandler<FundsListRequest, FundsListResponse>
    {
        // COMMENT: private field to hold an instance of the assessment repository interface.
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<FundsListRequestHandler> _logger;


        // COMMENT: Constructor:
        public FundsListRequestHandler(IAssessmentRepository assessmentRepository, ILogger<FundsListRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<FundsListResponse> Handle(FundsListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // COMMENT: get all funds from the assessment repository and store them in the funds variable.
                List<AssessmentFund> funds = await _assessmentRepository.GetAllFunds();

                // COMMENT: return a new FundsListResponse object containing the list of funds. 
                return new FundsListResponse(funds);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Unable to get list of funds.");
            }
        }
    }
}
