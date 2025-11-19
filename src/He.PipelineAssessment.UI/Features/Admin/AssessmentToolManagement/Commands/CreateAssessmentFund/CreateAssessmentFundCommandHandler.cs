using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentFunds;
using He.PipelineAssessment.UI.Features.Funds.ViewModels;
using MediatR;
using System.Reflection.Metadata;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentFund
{
    public class CreateAssessmentFundCommandHandler : IRequestHandler<CreateAssessmentFundCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<CreateAssessmentFundCommandHandler> _logger;

       public CreateAssessmentFundCommandHandler(IAssessmentRepository assessmentRepository, ILogger<CreateAssessmentFundCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<int> Handle(CreateAssessmentFundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentFund = CreateAssessmentFundCommandToAssessmentFund(request);
                await _assessmentRepository.CreateAssessmentFund(assessmentFund);

                return assessmentFund.Id;
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create assessment fund.");
            }
        }

        private AssessmentFund CreateAssessmentFundCommandToAssessmentFund(CreateAssessmentFundCommand assessmentFundCommand)
        {
            return new AssessmentFund
            {
                Name = assessmentFundCommand.Name,
                IsEarlyStage = assessmentFundCommand.IsEarlyStage,
                IsDisabled = assessmentFundCommand.IsDisabled
            };
        }
    }
}
