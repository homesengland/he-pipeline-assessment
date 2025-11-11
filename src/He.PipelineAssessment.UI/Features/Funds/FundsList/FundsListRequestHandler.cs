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
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<FundsListRequestHandler> _logger;


        public FundsListRequestHandler(IAssessmentRepository assessmentRepository, ILogger<FundsListRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<FundsListResponse> Handle(FundsListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var funds = await _assessmentRepository.GetAllFunds();

                if (funds == null)
                {
                    _logger.LogError("Database returned null when retrieving list of funds.");
                    funds = new List<AssessmentFund>();
                }

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
